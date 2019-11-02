using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Services;

namespace Web.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGroupStore _groupStore;
        public GameHub(IGroupStore groupStore)
        {
            _groupStore = groupStore ?? throw new ArgumentNullException(nameof(groupStore));
        }

        public async Task CreateNewGame()
        {
            // TODO make this code unique
            var uniqueCode = "ABCDEF";
            await Groups.AddToGroupAsync(Context.ConnectionId, uniqueCode);

            _groupStore.AddGroupHost(uniqueCode, Context.ConnectionId);

            await Clients.Caller.SendAsync(ClientMethods.OnCodeSet, uniqueCode);
        }

        public async Task JoinGame(string gameId, string userName)
        {
            if (!_groupStore.GroupExists(gameId))
            {
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.Client(_groupStore.GetGroupHost(gameId)).SendAsync(ClientMethods.OnPlayerJoined, userName);
        }
    }
}