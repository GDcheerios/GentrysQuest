using System;
using DiscordRPC;
using osu.Framework.Graphics.Containers;

namespace GentrysQuest.Game.Online
{
    public partial class DiscordRpc : CompositeComponent, IDisposable
    {
        private readonly DiscordRpcClient client;
        private bool disposed;

        public DiscordRpc(string applicationId)
        {
            client = new DiscordRpcClient(applicationId);
            client.Initialize();
        }

        public void UpdatePresence(string details, string state)
        {
            if (disposed) return;

            client.SetPresence(new RichPresence
            {
                Details = details,
                State = state,
                Timestamps = Timestamps.Now
            });
        }

        public new void Dispose()
        {
            if (disposed) return;
            disposed = true;

            client?.Dispose();
        }
    }
}
