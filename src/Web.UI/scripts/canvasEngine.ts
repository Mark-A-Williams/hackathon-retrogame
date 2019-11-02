import { Vector, GameState, Ball, Player } from './models';

export class CanvasEngine {
    public gameState: GameState;

    ctx: CanvasRenderingContext2D;
    ballRadius: number = 5;
    canvasSize: number = 300;
    sideLength: number;
    paddleLength: number = 0.3;
    vertices: Vector[];

    public drawFrame(gameState: GameState) {
        const canvas: HTMLCanvasElement = document.getElementById("canvas") as HTMLCanvasElement;

        this.ctx = canvas.getContext('2d');

        canvas.width = this.canvasSize;
        canvas.height = this.canvasSize;

        let numberOfPlayers = gameState.players.length;

        this.sideLength = this.calculateSideLength(numberOfPlayers);
        this.drawShape(numberOfPlayers);

        let position: Vector = {
            x: gameState.ball.xPosition,
            y: gameState.ball.yPosition
        };

        this.drawBall(position);

        this.drawPaddles(gameState.players);
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
                paddlePosition: Math.random()
            }

            players.push(player);
        }

        return players;
    }
    public calculateSideLength(numberOfPlayers: number): number {
        // 2 * Radius * Sin(PI / Number of players)
        return this.canvasSize * Math.sin(Math.PI/numberOfPlayers)
    }

    public drawPaddles(players: Player[]): void{
        let i;
        for (i = 0; i < players.length; i++)
        {
            let startVertex: Vector;
            let endVertex: Vector;
            let paddlePostion = players[i].paddlePosition * (1 - this.paddleLength) + this.paddleLength / 2;

            if (i !== players.length - 1)
            {
                startVertex = this.vertices[i]
                endVertex = this.vertices[i + 1]
            } else {
                startVertex = this.vertices[i]
                endVertex = this.vertices[0]
            }

            const deltaX = endVertex.x - startVertex.x;
            const deltaY = endVertex.y - startVertex.y;

            const paddleStart: Vector = {
                x: deltaX * (paddlePostion - (this.paddleLength / 2)) + startVertex.x,
                y: deltaY * (paddlePostion - (this.paddleLength / 2)) + startVertex.y
            }

            const paddleEnd: Vector = {
                x: deltaX * (paddlePostion + (this.paddleLength / 2)) + startVertex.x,
                y: deltaY * (paddlePostion + (this.paddleLength / 2)) + startVertex.y
            }

            this.drawLine(paddleStart, paddleEnd);
        }
    }
    public setVertices(numberOfSides: number): void
    {
        let n: number;
        let i: number;
        const radius = this.canvasSize / 2;
        this.vertices = [];
        for (n = 0; n < numberOfSides; n++)
        {
            let vertex: Vector = {
                x: radius * Math.sin(2 * Math.PI * (-1 * n / numberOfSides) + Math.PI + (Math.PI / numberOfSides)) + radius,
                y: radius * Math.cos(2 * Math.PI * (-1 * n / numberOfSides) + Math.PI + (Math.PI / numberOfSides)) + radius
            }

            this.vertices.push(vertex);
        }
    }

    public drawShape(numberOfSides: number) : void
    {
        let i: number;
        const radius = this.canvasSize / 2;
        this.setVertices(numberOfSides);

        this.ctx.strokeStyle = "red";
        for (i = 0; i < numberOfSides - 1; i++)
        {
            this.drawLine(this.vertices[i], this.vertices[i+1])
        }

        this.drawLine(this.vertices[numberOfSides - 1], this.vertices[0]);
        this.ctx.strokeStyle = "black";

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