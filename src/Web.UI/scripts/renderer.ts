export class Renderer {
    private readonly visibleName: string = 'visible';

    private initialised: boolean = false;

    private menuView: HTMLDivElement;
    private paddleView: HTMLDivElement;
    private gameView: HTMLDivElement;
    private gameCodeElement: HTMLSpanElement;

    constructor() {
        window.addEventListener("load", () => this.init());
    }

    public showMenu(): void {
        if (!this.initialised) {
            window.addEventListener("load", () => this.showMenu());
            return;
        }

        this.show(this.menuView);
        this.hide(this.gameView);
        this.hide(this.paddleView);
    }

    public showPaddle(): void {
        if (!this.initialised) {
            window.addEventListener("load", () => this.showPaddle());
            return;
        }

        this.hide(this.menuView);
        this.show(this.paddleView);
        this.hide(this.gameView);
    }

    public showGame(): void {
        if (!this.initialised) {
            window.addEventListener("load", () => this.showGame());
            return;
        }

        this.hide(this.menuView);
        this.hide(this.paddleView);
        this.show(this.gameView);
    }

    public renderGameCode(code: string): void {
        if (!this.initialised) {
            window.addEventListener('load', () => this.renderGameCode(code));
            return;
        }

        this.gameCodeElement.textContent = code;
    }

    private init(): void {
        this.menuView = document.querySelector('#menu-view');
        this.paddleView = document.querySelector('#paddle-view');
        this.gameView = document.querySelector('#game-view');

        this.gameCodeElement = document.querySelector('#gameCode');

        this.initialised = true;
    }

    private show(el: HTMLElement): void {
        el.classList.add(this.visibleName);
    }

    private hide(el: HTMLElement): void {
        el.classList.remove(this.visibleName);
    }
}