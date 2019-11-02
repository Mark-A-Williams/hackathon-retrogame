import * as signalR from '@aspnet/signalr';
import { GameState } from '../models';

export namespace Connection
{
    export class ConnectionService
    {
        // Client methods
        private readonly OnCodeSetName: string = "onCodeSet";
        private readonly OnPlayerJoinedName: string = "onPlayerJoined";
        private readonly OnGameStateUpdateName: string = "onGameStateUpdate";

        // Server methods
        private readonly CreateNewGameName: string = "CreateNewGame";
        private readonly JoinGameName: string = "JoinGame";
        private readonly GetGameStateName: string = "GetGameState";

        private readonly _connection: signalR.HubConnection;
        private readonly _onCodeSetCallbacks: {(code: string) : void }[] = [];
        private readonly _onPlayerJoinedCallbacks: {(userName: string) : void }[] = [];
        private readonly _onGameStateUpdateCallbacks: {(gameState: GameState) : void }[] = [];

        public constructor() {
            this._connection = new signalR.HubConnectionBuilder()
                .withUrl('/hub')
                .build();

            this._connection.on(this.OnCodeSetName, (code: string) => this._onCodeSetCallbacks.forEach((f) => f(code)));
            this._connection.on(this.OnPlayerJoinedName, (userName: string) => this._onPlayerJoinedCallbacks.forEach((f) => f(userName)));
            this._connection.on(this.OnGameStateUpdateName, (gameState: GameState) => this._onGameStateUpdateCallbacks.forEach((f) => f(gameState)));

            this._connection.start().catch(err => console.log(err));
        }

        set onCodeSet(value: {(code: string) : void}) {
            this._onCodeSetCallbacks.push(value);
        }

        set onPlayerJoined(value: {(userName: string) : void}) {
            this._onPlayerJoinedCallbacks.push(value);
        }

        set onGameStateUpdate(value: {(gameState: GameState) : void}) {
            this._onGameStateUpdateCallbacks.push(value);
        }

        public CreateGame(): void {
            this._connection.invoke(this.CreateNewGameName);
        }

        public JoinGame(userName: string, gameCode: string): void {
            this._connection.invoke(this.JoinGameName, userName, gameCode);
        }

        public GetGameState(gameCode: string): void  {
            this._connection.invoke(this.GetGameStateName, gameCode);
        }
    }
}