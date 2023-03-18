namespace cah.blazor.Hubs
{
    public interface IChatClient
    {
        Task SendMessage(string user, string message);

        Task SendWelcomeMessageToAllOtherUsers(string newUserId, string username);

        Task SendUserDisconnectedMessage(string username);
    }
}
