import * as signalR from '@aspnet/signalr';

export namespace Connection
{
    export class ConnectionService
    {
        // Client methods
        private readonly OnCodeSetName: string = "onCodeSet";
        private readonly OnPlayerJoinedName: string = "onPlayerJoined";

        // Server methods
        private readonly CreateNewGameName: string = "CreateNewGame";
        private readonly JoinGameName: string = "JoinGame";

        private readonly _connection: signalR.HubConnection;
        private readonly _onCodeSetCallbacks: {(code: string) : void }[] = [];
        private readonly _onPlayerJoinedCallbacks: {(userName: string) : void }[] = [];

        public constructor() {
            this._connection = new signalR.HubConnectionBuilder()
                .withUrl('/hub')
                .build();

            this._connection.on(this.OnCodeSetName, (code: string) => this._onCodeSetCallbacks.forEach((f) => f(code)));
            this._connection.on(this.OnPlayerJoinedName, (userName: string) => this._onPlayerJoinedCallbacks.forEach((f) => f(userName)));

            this._connection.start().catch(err => console.log(err));
        }

        set onCodeSet(value: {(code: string) : void}) {
            this._onCodeSetCallbacks.push(value);
        }

        set onPlayerJoined(value: {(userName: string) : void}) {
            this._onPlayerJoinedCallbacks.push(value);
        }

        public CreateGame(): void {
            this._connection.invoke(this.CreateNewGameName);
        }

        public JoinGame(userName: string, gameCode: string): void {
            this._connection.invoke(this.JoinGameName, userName, gameCode);
        }
    }
}