using cah.blazor.SocketConstants;
using cah.models;
using cah.services.services;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.SignalR;


namespace cah.blazor.Hubs
{
	public class ChatHub : Hub
	{
		private static GameSettings _gameSettings = new GameSettings { GameState = GameState.Initial};

		private static Dictionary<string, string> Users = new Dictionary<string, string>();

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

			GameUser gu = _gameSettings.GameUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
			if (gu != null)
			{
				_gameSettings.GameUsers.Remove(gu);
			}

			await SendUserDisconnectedMessage(username);
			await SendGameUsers();
		}


		#region CHAT

		public async Task SendGameUsers()
		{
			await Clients.All.SendAsync(SocketConstantHelpers.GameSettings, _gameSettings);
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
			GameUser gu = _gameSettings.GameUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
			if (gu == null)
			{
				_gameSettings.GameUsers.Add(new GameUser
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


		public async Task ResetGameData()
		{
			_gameSettings.GameStarted = false;
			_gameSettings.GameSet = string.Empty;
			_gameSettings.SelectedSet = string.Empty;
			foreach (var gu in _gameSettings.GameUsers)
			{
				gu.IsHost = false;
			}
			_gameSettings.GameState = GameState.Initial;
			await SendGameUsers();
		}


		public async Task GetGameSettings()
		{
			await Clients.All.SendAsync(SocketConstantHelpers.GameSettings, _gameSettings);
		}


		public async Task GetCardSets()
		{
			List<string> cardSets = await _cardsService.GetSets();
			await Clients.Caller.SendAsync(SocketConstantHelpers.CardSet, cardSets);
		}


		public async Task PickSet(string selectedSet)
		{
			_gameSettings.SelectedSet = selectedSet;
			_gameSettings.GameState = GameState.SetPicked;
			await SendGameUsers();
		}


		public async Task StartGame()
		{
			GameUser gu = _gameSettings.GameUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
			_gameSettings.GameState = GameState.StartGame;
			gu.IsHost = true;
			await SendGameUsers();	
		}


		public async Task SetDealCardsState()
		{
			_gameSettings.GameState = GameState.CardsDealt;
			await SendGameUsers();
		}

		/// <summary>
		/// Selects the cards, and deals them to the players
		/// </summary>
		/// <returns></returns>
		public async Task DealCards()
		{
			_gameSettings.GameState = GameState.CardsDealt;
			string selectedSet = "CAH Base Set";
			if (_gameSettings != null && !string.IsNullOrWhiteSpace(_gameSettings.SelectedSet)) {
				selectedSet = _gameSettings.SelectedSet;
			}
			_gameSettings.GameState = GameState.CardsDealt;

			var whiteCards = await _cardsService.GetRandomWhiteCards(selectedSet, _gameSettings.GameUsers.Count);
			var blackCard = await _cardsService.GetRandomBlackCard(selectedSet);

			var chunkedWhiteCards = whiteCards.Chunk(_gameSettings.CardsPerPerson).ToList();
			

			foreach (var gameUser in _gameSettings.GameUsers.Select((item, index) => new { index, item }))
			{
				gameUser.item.WhiteCards = chunkedWhiteCards[gameUser.index].ToList();
				gameUser.item.BlackCard = blackCard;
				await Clients.Client(gameUser.item.ConnectionId).SendAsync(SocketConstantHelpers.PersonalGameCards, gameUser.item);
			}	
			await SendGameUsers();	
		}
		#endregion

	}
}
