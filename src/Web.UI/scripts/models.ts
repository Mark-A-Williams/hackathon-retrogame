
export class GameState {
    ball: Ball;
    players: Player[];
}

export class Ball {
    position: Vector;
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