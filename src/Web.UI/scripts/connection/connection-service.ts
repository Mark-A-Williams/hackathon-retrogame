import * as signalR from '@microsoft/signalr';
import { GameState } from '../models';

export namespace Connection
{
    export class ConnectionService
    {
        // Client methods
        private readonly OnCodeSetName: string = "onCodeSet";
        private readonly OnPlayerJoinedName: string = "onPlayerJoined";
        private readonly OnGameStateUpdateName: string = "onGameStateUpdate";
        private readonly OnGameStartName: string = "onGameStart";
        private readonly OnColourSetName: string = "onColourSet";

        // Server methods
        private readonly CreateNewGameName: string = "CreateNewGame";
        private readonly JoinGameName: string = "JoinGame";
        private readonly GetGameStateName: string = "GetGameState";
        private readonly UpdatePositionName: string = "UpdatePosition";
        private readonly StartGameName: string = "StartGame";

        private readonly _connection: signalR.HubConnection;

        // Callbacks for client methods
        private readonly _onCodeSetCallbacks: {(code: string) : void }[] = [];
        private readonly _onPlayerJoinedCallbacks: {(userName: string) : void }[] = [];
        private readonly _onGameStateUpdateCallbacks: {(gameState: GameState) : void }[] = [];
        private readonly _onGameStartCallbacks: {() : void}[] = [];
        private readonly _onColourSetCallbacks: {(colour: string) : void}[] = [];

        public constructor() {
            this._connection = new signalR.HubConnectionBuilder()
                .withUrl('/hub')
                .withAutomaticReconnect()
                .build();

            this._connection.on(this.OnCodeSetName, (code: string) => this._onCodeSetCallbacks.forEach((f) => f(code)));
            this._connection.on(this.OnPlayerJoinedName, (userName: string) => this._onPlayerJoinedCallbacks.forEach((f) => f(userName)));
            this._connection.on(this.OnGameStateUpdateName, (gameState: GameState) => this._onGameStateUpdateCallbacks.forEach((f) => f(gameState)));
            this._connection.on(this.OnGameStartName, () => this._onGameStartCallbacks.forEach(f => f()));
            this._connection.on(this.OnColourSetName, (colour: string) => this._onColourSetCallbacks.forEach(f => f(colour)));

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

        set onColourSet(value: {(colour: string) : void }) {
            this._onColourSetCallbacks.push(value);
        }

        public CreateGame(): void {
            this._connection.invoke(this.CreateNewGameName);
        }

        public JoinGame(userName: string, gameCode: string): void {
            this._connection.invoke(this.JoinGameName, gameCode, userName);
        }

        public RequestGameState(gameCode: string): Promise<void>  {
            return this._connection.invoke(this.GetGameStateName, gameCode);
        }

        public UpdatePosition(gameRoomName: string, position: number): void {
            this._connection.invoke(this.UpdatePositionName, gameRoomName, position);
        }

        public StartGame(gameCode: string): Promise<void> {
            return this._connection.invoke(this.StartGameName, gameCode);
        }
    }
}