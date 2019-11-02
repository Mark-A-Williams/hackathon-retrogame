
export class GameState {
    ball: Ball;
    players: Player[];
}

export class Ball {
    xPosition: number;
    yPosition: number;
    travel: Vector; 
}


export class Vector {
    x: number;
    y: number;
}

export class Player {
    index: number;
    color: string;
    paddlePosition: number;
}