namespace GentrysQuest.Game.Online.API.Requests
{
    public class CheckPlayerRequest(int id) : APIRequest<string>
    {
        public override string Target { get; } = $"gq/check-player/{id}";
    }
}
