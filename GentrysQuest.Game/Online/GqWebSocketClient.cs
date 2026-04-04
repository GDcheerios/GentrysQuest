using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GentrysQuest.Game.Online.API;
using Newtonsoft.Json;
using osu.Framework.Logging;

namespace GentrysQuest.Game.Online
{
    public class GqWebSocketClient : IDisposable
    {
        private readonly SemaphoreSlim sendGate = new(1, 1);
        private readonly SemaphoreSlim lifecycleGate = new(1, 1);
        private readonly int bufferSize;
        private ClientWebSocket socket;
        private CancellationTokenSource receiveLoopCancellation;
        private CancellationTokenSource reconnectLoopCancellation;
        private Task receiveTask;
        private Task reconnectTask;
        private bool disposed;
        private bool userRequestedDisconnect = true;
        private bool suppressReconnect;
        private string endpoint;
        private Func<CancellationToken, Task> postConnectAction;

        public event Action<string> OnMessage;
        public event Action<WebSocketState> OnStateChanged;
        public event Action<Exception> OnError;
        public event Action<int, TimeSpan> OnReconnectAttempt;

        public GqWebSocketClient(int bufferSize = 4096)
        {
            this.bufferSize = bufferSize;
        }

        public bool AutoReconnectEnabled { get; set; } = true;
        public int InitialReconnectDelayMs { get; set; } = 500;
        public int MaxReconnectDelayMs { get; set; } = 10000;

        public WebSocketState State => socket?.State ?? WebSocketState.None;
        public bool IsConnected => State == WebSocketState.Open;

        public async Task ConnectAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentException("WebSocket endpoint is required.", nameof(endpoint));

            if (disposed)
                throw new ObjectDisposedException(nameof(GqWebSocketClient));

            await lifecycleGate.WaitAsync(cancellationToken);
            try
            {
                if (IsConnected)
                    return;

                userRequestedDisconnect = false;
                this.endpoint = endpoint;
                suppressReconnect = true;
                await disconnectInternalAsync(WebSocketCloseStatus.NormalClosure, "Reconnecting", cancellationToken, false);
                cancelReconnectLoop();

                socket = new ClientWebSocket();
                var uri = new Uri(endpoint);
                Logger.Log($"Connecting websocket to {uri}", LoggingTarget.Network);

                await socket.ConnectAsync(uri, cancellationToken);
                onStateChanged();

                receiveLoopCancellation = new CancellationTokenSource();
                receiveTask = Task.Run(() => receiveLoop(receiveLoopCancellation.Token), CancellationToken.None);
            }
            finally
            {
                suppressReconnect = false;
                lifecycleGate.Release();
            }
        }

        public async Task DisconnectAsync(WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure, string closeStatusDescription = "Closing connection", CancellationToken cancellationToken = default)
        {
            await lifecycleGate.WaitAsync(cancellationToken);
            try
            {
                userRequestedDisconnect = true;
                cancelReconnectLoop();
                await disconnectInternalAsync(closeStatus, closeStatusDescription, cancellationToken, true);
            }
            finally
            {
                lifecycleGate.Release();
            }
        }

        public async Task ConnectAndAuthenticateAsync(CancellationToken cancellationToken = default)
        {
            await ConnectAndAuthenticateAsync(APIAccess.Endpoint.WebsocketUrl, cancellationToken);
        }

        public async Task ConnectAndAuthenticateAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            postConnectAction = authenticateWithCurrentSessionAsync;
            await ConnectAsync(endpoint, cancellationToken);
            await postConnectAction(cancellationToken);
        }

        private async Task authenticateWithCurrentSessionAsync(CancellationToken cancellationToken)
        {
            await APIAccess.EnsureApiKeyAsync();

            var token = APIAccess.GetUserToken();
            var apiKey = APIAccess.GetApiKey()?.GetHeader();
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("Cannot authenticate websocket without user token and API key.");

            var payload = new
            {
                type = "authenticate",
                token,
                api_key = apiKey
            };

            await SendJsonAsync(payload, cancellationToken);
            Logger.Log("Sent websocket auth payload.", LoggingTarget.Network);
        }

        private async Task disconnectInternalAsync(WebSocketCloseStatus closeStatus, string closeStatusDescription, CancellationToken cancellationToken, bool clearPostConnectAction)
        {
            receiveLoopCancellation?.Cancel();

            if (receiveTask != null)
            {
                try
                {
                    await receiveTask;
                }
                catch (OperationCanceledException)
                {
                }
            }

            if (socket == null)
                return;

            try
            {
                if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived)
                    await socket.CloseAsync(closeStatus, closeStatusDescription, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to close websocket cleanly: {ex}", LoggingTarget.Network, LogLevel.Important);
                OnError?.Invoke(ex);
            }
            finally
            {
                socket.Dispose();
                socket = null;
                receiveLoopCancellation?.Dispose();
                receiveLoopCancellation = null;
                receiveTask = null;
                if (clearPostConnectAction)
                    postConnectAction = null;

                onStateChanged();
            }
        }

        public async Task SendTextAsync(string message, CancellationToken cancellationToken = default)
        {
            if (!IsConnected || socket == null)
                throw new InvalidOperationException("WebSocket is not connected.");

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var bytes = Encoding.UTF8.GetBytes(message);
            await sendGate.WaitAsync(cancellationToken);
            try
            {
                await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);
            }
            finally
            {
                sendGate.Release();
            }
        }

        public Task SendJsonAsync<T>(T data, CancellationToken cancellationToken = default)
        {
            var payload = JsonConvert.SerializeObject(data);
            return SendTextAsync(payload, cancellationToken);
        }

        private async Task receiveLoop(CancellationToken cancellationToken)
        {
            var buffer = new byte[bufferSize];
            using var stream = new MemoryStream();

            try
            {
                while (!cancellationToken.IsCancellationRequested && socket != null && socket.State == WebSocketState.Open)
                {
                    stream.SetLength(0);

                    WebSocketReceiveResult result;
                    do
                    {
                        result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            onStateChanged();
                            return;
                        }

                        stream.Write(buffer, 0, result.Count);
                    } while (!result.EndOfMessage);

                    var message = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                    OnMessage?.Invoke(message);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Logger.Log($"WebSocket receive loop failed: {ex}", LoggingTarget.Network, LogLevel.Important);
                OnError?.Invoke(ex);
            }
            finally
            {
                onStateChanged();
                if (!userRequestedDisconnect && !suppressReconnect)
                    startReconnectLoop();
            }
        }

        private void startReconnectLoop()
        {
            if (!AutoReconnectEnabled || string.IsNullOrWhiteSpace(endpoint) || disposed)
                return;

            if (reconnectTask != null && !reconnectTask.IsCompleted)
                return;

            reconnectLoopCancellation?.Dispose();
            reconnectLoopCancellation = new CancellationTokenSource();
            reconnectTask = Task.Run(() => reconnectLoopAsync(reconnectLoopCancellation.Token), CancellationToken.None);
        }

        private async Task reconnectLoopAsync(CancellationToken cancellationToken)
        {
            var attempt = 0;

            while (!cancellationToken.IsCancellationRequested && !disposed && !userRequestedDisconnect)
            {
                attempt++;
                var delayMs = Math.Min(MaxReconnectDelayMs, (int)(InitialReconnectDelayMs * Math.Pow(2, attempt - 1)));
                var delay = TimeSpan.FromMilliseconds(delayMs);
                OnReconnectAttempt?.Invoke(attempt, delay);
                Logger.Log($"WebSocket reconnect attempt #{attempt} in {delay.TotalMilliseconds:0}ms", LoggingTarget.Network);

                try
                {
                    await Task.Delay(delay, cancellationToken);
                    await ConnectAsync(endpoint, cancellationToken);

                    if (postConnectAction != null)
                        await postConnectAction(cancellationToken);

                    Logger.Log("WebSocket reconnected successfully.", LoggingTarget.Network);
                    return;
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Logger.Log($"WebSocket reconnect attempt #{attempt} failed: {ex.Message}", LoggingTarget.Network, LogLevel.Important);
                    OnError?.Invoke(ex);
                }
            }
        }

        private void cancelReconnectLoop()
        {
            reconnectLoopCancellation?.Cancel();
            reconnectLoopCancellation?.Dispose();
            reconnectLoopCancellation = null;
            reconnectTask = null;
        }

        private void onStateChanged()
        {
            OnStateChanged?.Invoke(State);
            Logger.Log($"WebSocket state: {State}", LoggingTarget.Network);
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;

            try
            {
                DisconnectAsync().GetAwaiter().GetResult();
            }
            finally
            {
                cancelReconnectLoop();
                sendGate.Dispose();
                lifecycleGate.Dispose();
            }
        }
    }
}
