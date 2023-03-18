using cah.blazor.SocketConstants;
using cah.models;
using cah.services.services;
using Microsoft.AspNetCore.SignalR;

namespace cah.blazor.Hubs
{
    public class GameHub : Hub
    {

        private static List<GameUser> _gameUsers = new List<GameUser>();
        private readonly ICardsService _cardsService;

        public GameHub(ICardsService cardService)
        {
            _cardsService = cardService;    
        }


        public override async Task OnConnectedAsync()
        {
            GameUser gu = new GameUser
            {
                Username = Context.GetHttpContext().Request.Query["username"],
                ConnectionId = Context.ConnectionId
            };
            _gameUsers.Add(gu);

            await SendGameUsers();

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            GameUser gu = _gameUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (gu != null)
            {
                _gameUsers.Remove(gu);
            }
        }





        public async Task SendGameUsers()
        {
            await Clients.All.SendAsync(SocketConstantHelpers.GameUsers, _gameUsers);
        }


    }
}
