using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Services;

namespace Web.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGroupStore _groupStore;
        private readonly IPlayerStore _playerStore;
        private readonly IGameEngineService _gameEngineService;

        public GameHub(
            IGroupStore groupStore,
            IPlayerStore playerStore,
            IGameEngineService gameEngineService)
        {
            _groupStore = groupStore ?? throw new ArgumentNullException(nameof(groupStore));
            _playerStore = playerStore ?? throw new ArgumentNullException(nameof(playerStore));
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
                throw new ArgumentException($"There is no game '{gameId}'.");
            }

            if (!_gameEngineService.CanJoinGame(gameId))
            {
                throw new InvalidOperationException($"Game {gameId} has already started.");
            }

            var addResult = _gameEngineService.AddPlayer(gameId, userName);
            var player = new PlayerDetails
            {
                Id = addResult.Id,
                Color = addResult.Color,
                UserName = userName
            };

            _playerStore.AddPlayer(Context.ConnectionId, player);

            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await Clients.Client(_groupStore.GetGroupHost(gameId)).SendAsync(ClientMethods.OnPlayerJoined, userName);
            await Clients.Caller.SendAsync(ClientMethods.OnColourSet, player.Color);
        }

        public async Task GetGameState(string gameId)
        {
            if (!_groupStore.GroupExists(gameId))
            {
                return;
            }

            var gameState = _gameEngineService.GetGameState(gameId);

            await Clients.Caller.SendAsync(ClientMethods.OnGameStateUpdate, gameState);
        }

        public void UpdatePosition(string gameId, float position)
        {
            if (!_groupStore.GroupExists(gameId))
            {
                return;
            }

            var playerId = _playerStore.GetPlayer(Context.ConnectionId).Id;

            _gameEngineService.MovePlayer(gameId, playerId, position);
        }

        public async Task StartGame(string gameId)
        {
            if (!_groupStore.GroupExists(gameId))
            {
                return;
            }

            var gameState = _gameEngineService.GetGameState(gameId);
            if (gameState.Players.Count < 2)
            {
                throw new InvalidOperationException();
            }

            _gameEngineService.StartGame(gameId);

            await Clients.Group(gameId).SendAsync(ClientMethods.OnGameStart);
        }
    }
}
