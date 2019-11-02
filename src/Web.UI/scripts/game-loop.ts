import { Connection } from "./connection/connection-service";

const DelayMs = 300;

export class GameLoop {
    constructor(
        private readonly _code: string,
        private readonly _connSvc: Connection.ConnectionService) {
    }

    private _active: boolean;
    private _lastUpdated: Date;

    public start(): void {
        this._active = true;
        this.tick();
    }

    public stop(): void {
        this._active = false;
    }

    private tick(): void {
        if (!this._active) {
            return;
        }

        const requestTimestamp = new Date();

        this._connSvc.RequestGameState(this._code)
            .then(() => {
                if (this._active) {
                    const timeTaken: number = <any>(new Date()) - <any>requestTimestamp;

                    window.setTimeout(() => {
                        this.tick();
                    }, DelayMs - timeTaken);
                }
            });
    }
}
