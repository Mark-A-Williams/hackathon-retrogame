export class PaddleHandler {
    private paddleSlider: HTMLInputElement;
    private _paddleMoveCallbacks: {(position: number) : void}[] = [];

    private initialised: boolean = false;

    constructor() {
        window.addEventListener('load', () => this.init());
    }

    private init() {
        this.paddleSlider = document.querySelector('#paddle');
        this.paddleSlider.addEventListener('input', () => {
            const position = +(this.paddleSlider.value);

            this._paddleMoveCallbacks.forEach(f => f(position));
        });

        this.initialised = true;
    }

    public setColour(colour: string): void {
        if (!this.initialised) {
            window.addEventListener('load', () => this.setColour(colour));
            return;
        }

        this.paddleSlider.style.backgroundColor = colour;
    }

    set onPaddleMove(value: {(position: number) : void}) {
        this._paddleMoveCallbacks.push(value);
    }
}