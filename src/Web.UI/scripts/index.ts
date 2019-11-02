import '../styles/main.css';
import * as signalR from '@microsoft/signalr'
import { CanvasEngine } from './canvasEngine';
import { GameState } from './models';
import { Connection } from './connection/connection-service';
import { Renderer } from './renderer';
import { MenuHandler } from './menu-handler';
import { PaddleHandler } from './paddle-handler';
import { GameLoop } from './game-loop';

let gameCode = '';
const canvasEngine = new CanvasEngine();
const renderer = new Renderer();
const menuHandler = new MenuHandler();
const paddleHandler = new PaddleHandler();
const connection = new Connection.ConnectionService();

menuHandler.onJoinGameSubmit = (code, username) => {
    connection.onColourSet = (colour) => {
        gameCode = code;
        paddleHandler.setColour(colour);
        paddleHandler.onPaddleMove = (position) => connection.UpdatePosition(gameCode, position);
        renderer.showPaddle();
    };
    
    connection.JoinGame(username, code)
};

menuHandler.onNewGameClicked = () => {
    connection.onCodeSet = (code) => {
        gameCode = code;
        const looper = new GameLoop(code, connection);
        looper.start();
        connection.onGameStateUpdate = (state) => {
            console.log(state);
            canvasEngine.drawFrame(state);
        }
        renderer.renderGameCode(gameCode);
        renderer.showGame();

        menuHandler.onStartGameClick = () => {
            connection.StartGame(gameCode)
                .then(() => {
                    menuHandler.hideStartGameButton();
                })
        }
    };

    connection.CreateGame();
};

renderer.showMenu();
