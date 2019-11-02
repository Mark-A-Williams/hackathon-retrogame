import { Vector, GameState, Ball, Player } from './models';

export class CanvasEngine {
    public gameState: GameState;

    ctx: CanvasRenderingContext2D;
    ballRadius: number = 5;
    canvasSize: number = 300;
    sideLength: number;

    public drawFrame(gameState: GameState) {
        const canvas: HTMLCanvasElement = document.getElementById("canvas") as HTMLCanvasElement;

        this.ctx = canvas.getContext('2d');

        canvas.width = this.canvasSize;
        canvas.height = this.canvasSize;

        let numberOfPlayers = gameState.players.length;

        this.drawShape(numberOfPlayers);
        this.sideLength = this.calculateSideLength(numberOfPlayers);

        let position: Vector = {
            x: gameState.ball.xPosition,
            y: gameState.ball.yPosition
        };

        this.drawBall(position);
    }

    public createMockGamestate(numberOfPlayers: number): GameState {

        const mockBall: Ball = {
            xPosition: (this.canvasSize / 2) * (Math.random() * 2 - 1 ) + this.canvasSize / 2,
            yPosition: (this.canvasSize / 2) * (Math.random() * 2 - 1 ) + this.canvasSize / 2,
            travel: null
        }

        const mockPlayers = this.createMockPlayers(numberOfPlayers);

       return {
           ball: mockBall,
           players: mockPlayers
       }
    }
    public createMockPlayers(numberOfPlayers: number): Player[] {
        let i: number;
        let players: Player[] = [];

        for (i = 0; i < numberOfPlayers; i++)
        {
            const player: Player = {
                index: i,
                color: "#000",
                paddlePosition: Math.random() * (0.85 - 0.15) + 0.15
            }

            players.push(player);
        }

        return players
    }
    public calculateSideLength(numberOfPlayers: number): number {
        // 2 * Radius * Sin(PI / Number of players)
        return this.canvasSize * Math.sin(Math.PI/numberOfPlayers)
    }

    public drawPaddles(players: Player[]){
        for (const player of players)
        {
            this.drawPaddle(player.paddlePosition);
        }
    }
    public drawPaddle(paddlePosition: number) {
        
    }

    public drawShape(numberOfSides: number)
    {
        let vertices: Vector[] = [];
        let n: number;
        let i: number;
        const radius = this.canvasSize / 2;

        for (n = 0; n < numberOfSides; n++)
        {
            let vertex: Vector = {
                x: radius * Math.cos(2 * Math.PI * n / numberOfSides) + radius,
                y: radius * Math.sin(2 * Math.PI * n / numberOfSides) + radius
            }
            vertices.push(vertex);
        }

        for (i = 0; i < numberOfSides - 1; i++)
        {
            this.drawLine(vertices[i], vertices[i+1])
        }

        this.drawLine(vertices[numberOfSides - 1], vertices[0]);
    }

    public drawBall(position: Vector)
    {
        this.drawCircle(position);
    }

    public drawLine(start: Vector, end: Vector)
    {
        this.ctx.beginPath();
        this.ctx.moveTo(start.x, start.y);
        this.ctx.lineTo(end.x, end.y);
        this.ctx.stroke();
        this.ctx.closePath();
    }

    public drawCircle(position: Vector)
    {
        this.ctx.beginPath();
        this.ctx.arc(position.x, position.y, this.ballRadius, 0, 2*Math.PI, false);
        this.ctx.stroke();
        this.ctx.closePath();
    }
}