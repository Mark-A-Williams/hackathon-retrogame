export class PaddleHandler {
    private paddleSlider: HTMLInputElement;
    private _paddleMoveCallbacks: {(position: number) : void}[] = [];

    constructor() {
        window.addEventListener('load', () => this.init());
    }

    private init() {
        this.paddleSlider = document.querySelector('#paddle');
        this.paddleSlider.addEventListener('input', () => {
            const position = +(this.paddleSlider.value);

            this._paddleMoveCallbacks.forEach(f => f(position));
        });
    }

    set onPaddleMove(value: {(position: number) : void}) {
        this._paddleMoveCallbacks.push(value);
    }
}