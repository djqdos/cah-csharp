using cah.blazor.SocketConstants;
using Microsoft.AspNetCore.SignalR;


namespace cah.blazor.Hubs
{
	public class ChatHub : Hub
	{
		private static Dictionary<string, string> Users = new Dictionary<string, string>();

		public override async Task OnConnectedAsync()
		{
			string username = Context.GetHttpContext().Request.Query["username"];
			Users.Add(Context.ConnectionId, username);
			await SendWelcomeMessageToAllOtherUsers(Context.ConnectionId, username);
			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			string username = Users.FirstOrDefault(x => x.Key == Context.ConnectionId).Value;			
			await SendUserDisconnectedMessage(username);			
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
		/// Sends a message to all client, except the instigator
		/// </summary>
		/// <param name="newUserId"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		public async Task SendWelcomeMessageToAllOtherUsers(string newUserId, string username)
		{
			await Clients.AllExcept(new[] { newUserId })
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
			await Clients.All.SendAsync(SocketConstantHelpers.UserLeft, 
										SocketConstantHelpers.ChatBotName, 
										$"{username} has left the chat");
		}
	}
}
