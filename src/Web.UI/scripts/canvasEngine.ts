class CanvasEngine {
    public gameState: GameState;

    ctx: CanvasRenderingContext2D;

    public drawFrame(gameState: GameState) {
        const canvas: HTMLCanvasElement = document.getElementById("canvas") as HTMLCanvasElement;

        this.ctx = canvas.getContext('2d');

        canvas.width = 400;
        canvas.height = 400;
        this.drawShape(5);
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

    public drawLine(start: Vector, end: Vector)
    {
        this.ctx.beginPath();
        this.ctx.moveTo(start.x, start.y);
        this.ctx.lineTo(end.x, end.y);
        this.ctx.stroke();
        this.ctx.closePath();
    }
}

const init = () => {
    let canvasEngine = new CanvasEngine;
    let button = document.getElementById("button").addEventListener("click", () => canvasEngine.drawFrame(null));
}

class GameState {
    ball: Ball;
    players: Player[];
}

class Ball {
    xPostion: number;
    yPosition: number;
    travel: Vector; 
}


class Vector {
    x: number;
    y: number;
}

class Player {
    index: number;
    color: Color;
    paddlePosition: number;
}

enum Color {
    red = 0,
    blue = 1
}