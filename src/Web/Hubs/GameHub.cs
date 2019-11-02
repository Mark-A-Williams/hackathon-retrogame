using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Services;

namespace Web.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGroupStore _groupStore;
        private readonly GameEngineService _gameEngineService;

        public GameHub(
            IGroupStore groupStore,
            GameEngineService gameEngineService)
        {
            _groupStore = groupStore ?? throw new ArgumentNullException(nameof(groupStore));
            _gameEngineService = gameEngineService ?? throw new ArgumentNullException(nameof(gameEngineService));
        }

        public async Task CreateNewGame()
        {
            var game = _gameEngineService.AddGame();

            await Groups.AddToGroupAsync(Context.ConnectionId, game.Code);

            _groupStore.AddGroupHost(game.Code, Context.ConnectionId);

            await Clients.Caller.SendAsync(ClientMethods.OnCodeSet, game.Code);
        }

        public async Task JoinGame(string gameId, string userName)
        {
            if (!_groupStore.GroupExists(gameId))
            {
                return;
            }

            var addResult = _gameEngineService.AddPlayer(gameId, userName);

            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.Client(_groupStore.GetGroupHost(gameId)).SendAsync(ClientMethods.OnPlayerJoined, userName);
        }
    }
}
