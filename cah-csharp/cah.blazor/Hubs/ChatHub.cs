using cah.blazor.SocketConstants;
using cah.models;
using cah.services.services;
using Microsoft.AspNetCore.SignalR;


namespace cah.blazor.Hubs
{
	public class ChatHub : Hub
	{
		private static Dictionary<string, string> Users = new Dictionary<string, string>();

		private static List<GameUser> _gameUsers = new List<GameUser>();
		private readonly ICardsService _cardsService;

		public ChatHub(ICardsService cardService)
		{
			_cardsService = cardService;

		}

        public override async Task OnConnectedAsync()
		{					
			await base.OnConnectedAsync();
		}


		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			string username = Users.FirstOrDefault(x => x.Key == Context.ConnectionId).Value;

			GameUser gu = _gameUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
			if (gu != null)
			{
				_gameUsers.Remove(gu);
			}

			await SendUserDisconnectedMessage(username);
			await SendGameUsers();
		}


		#region CHAT

		public async Task SendGameUsers()
		{
			await Clients.All.SendAsync(SocketConstantHelpers.GameUsers, _gameUsers);
		}


		/// <summary>
		/// Send a message to all clients
		/// </summary>
		/// <param name="user"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public async Task SendMessage(string user, string message)
		{
			await Clients.All.SendAsync(SocketConstantHelpers.RecieveMessage, user, message);
		}


		/// <summary>
		/// Updates the users' username, when it's beenn read from the cookie.
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public async Task UpdateUsername(string username)
		{

			// game user
			GameUser gu = _gameUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
			if (gu == null)
			{
				_gameUsers.Add(new GameUser
				{
					Username = username,
					ConnectionId = Context.ConnectionId
				});
			}
			else
			{
				gu.Username = username;
			}


			// chat user
			if (!Users.ContainsKey(Context.ConnectionId))
			{
				Users.Add(Context.ConnectionId, username);
			}

			await SendWelcomeMessageToAllOtherUsers(Context.ConnectionId, username);
			await SendGameUsers();
		}



		/// <summary>
		/// Sends a message to all client, except the instigator
		/// </summary>
		/// <param name="newUserId"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		public async Task SendWelcomeMessageToAllOtherUsers(string newUserId, string username)
		{	
			await Clients.Others
				.SendAsync(SocketConstantHelpers.UserJoined,
                           SocketConstantHelpers.ChatBotName,
                           $"{username} joined the chat!");
		}



		/// <summary>
		/// Send a disconnect message to all users
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public async Task SendUserDisconnectedMessage(string username)
		{
			await Clients.Others.SendAsync(SocketConstantHelpers.UserLeft, 
										SocketConstantHelpers.ChatBotName, 
										$"{username} has left the chat");
		}

		#endregion



		#region GAME
		public async Task StartGame()
		{
			var whiteCards = await _cardsService.GetRandomWhiteCards("CAH Base Set", _gameUsers.Count);
			var blackCard = await _cardsService.GetRandomBlackCard("CAH Base Set");

			var chunkedWhiteCards = whiteCards.Chunk(GameSettings.CardsPerPerson).ToList();
			

			foreach (var gameUser in _gameUsers.Select((item, index) => new { index, item }))
			{
				gameUser.item.WhiteCards = chunkedWhiteCards[gameUser.index].ToList();
				gameUser.item.BlackCard = blackCard;
				await Clients.Client(gameUser.item.ConnectionId).SendAsync(SocketConstantHelpers.PersonalGameCards, gameUser.item);
			}
			
			


		}
		#endregion

	}
}
