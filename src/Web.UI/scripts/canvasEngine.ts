import { Vector, GameState } from './models';

export class CanvasEngine {
    public gameState: GameState;

    ctx: CanvasRenderingContext2D;
    ballRadius: number = 2;


    public drawFrame(gameState: GameState) {
        const canvas: HTMLCanvasElement = document.getElementById("canvas") as HTMLCanvasElement;

        this.ctx = canvas.getContext('2d');

        canvas.width = 400;
        canvas.height = 400;
        this.drawShape(5);

        let position: Vector;
        position.x = 100 * Math.random()
        position.y = 100 * Math.random()

        this.drawBall(position);
    }

    public drawShape(numberOfSides: number)
    {
        let vertices: Vector[] = [];
        let n: number;
        let i: number;
        const radius = 200;

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
        this.ctx.moveTo(position.x, position.y);
        this.ctx.arc(position.x, position.y, this.ballRadius, 0, 2*Math.PI, false);
    }
}