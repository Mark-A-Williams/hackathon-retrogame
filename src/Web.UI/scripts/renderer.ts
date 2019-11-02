export class Renderer {
    private readonly visibleName: string = 'visible';

    private initialised: boolean = false;

    private menuView: HTMLDivElement;
    private paddleView: HTMLDivElement;
    private gameView: HTMLDivElement;

    constructor() {
        window.addEventListener("load", () => this.init());
    }

    public showMenu(): void {
        if (!this.initialised) {
            window.addEventListener("load", () => this.showMenu());
        }

        this.show(this.menuView);
        this.hide(this.gameView);
        this.hide(this.paddleView);
    }

    public showPaddle(): void {
        if (!this.initialised) {
            window.addEventListener("load", () => this.showPaddle());
        }

        this.hide(this.menuView);
        this.show(this.paddleView);
        this.hide(this.gameView);
    }

    public showGame(): void {
        if (!this.initialised) {
            window.addEventListener("load", () => this.showGame());
        }

        this.hide(this.menuView);
        this.hide(this.paddleView);
        this.show(this.gameView);
    }

    private init(): void {
        this.menuView = document.querySelector('#menu-view');
        this.paddleView = document.querySelector('#paddle-view');
        this.gameView = document.querySelector('#game-view');

        this.initialised = true;
    }

    private show(el: HTMLElement): void {
        el.classList.add(this.visibleName);
    }

    private hide(el: HTMLElement): void {
        el.classList.remove(this.visibleName);
    }
}