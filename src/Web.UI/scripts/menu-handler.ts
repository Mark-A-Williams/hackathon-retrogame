export class MenuHandler {

    private newGameButton: HTMLButtonElement;
    private gameCodeInput: HTMLInputElement;
    private userNameInput: HTMLInputElement;
    private joinGameForm: HTMLFormElement;

    private _newGameCallbacks: {() : void}[] = [];
    private _joinGameCallbacks: {(gameCode: string, username: string) : void}[] = [];

    constructor() {
        window.addEventListener("load", () => this.init());
    }

    private init(): void {
        this.newGameButton = document.querySelector('#newGameButton');
        this.gameCodeInput = document.querySelector('#gameCodeInput');
        this.userNameInput = document.querySelector('#userNameInput');
        this.joinGameForm = document.querySelector('#joinGameForm');

        this.newGameButton.addEventListener('click', () => {
            this._newGameCallbacks.forEach(f => f());
        });

        this.joinGameForm.addEventListener('submit', (e: Event) => {
            e.preventDefault();
            
            const userName = this.userNameInput.value;
            const gameCode = this.gameCodeInput.value;

            if (!userName.length || !gameCode.length) {
                // TODO validation messages
                return;
            }

            this._joinGameCallbacks.forEach(f => f(gameCode, userName));
        });
    }

    set onNewGameClicked(value: {() : void}) {
        this._newGameCallbacks.push(value);
    }
    
    set onJoinGameSubmit(value: {(gameCode: string, username: string) : void}) {
        this._joinGameCallbacks.push(value);
    }
}