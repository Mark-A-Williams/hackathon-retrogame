import '../styles/main.css';
import * as signalR from '@aspnet/signalr';
import { CanvasEngine } from './canvasEngine';
import { GameState } from './models';
import { Connection } from './connection/connection-service';
import { Renderer } from './renderer';
import { MenuHandler } from './menu-handler';
import { PaddleHandler } from './paddle-handler';

let gameCode = '';
const btnFrame: HTMLButtonElement = document.querySelector('#frameButton');
const canvasEngine = new CanvasEngine();
const renderer = new Renderer();
const menuHandler = new MenuHandler();
const paddleHandler = new PaddleHandler();
const connection = new Connection.ConnectionService();

btnFrame.addEventListener('click', () => {
  const gameState = canvasEngine.createMockGamestate(5);
  canvasEngine.drawFrame(gameState);
});

menuHandler.onJoinGameSubmit = (code, username) => {
  gameCode = code;
  connection.JoinGame(username, gameCode);
  paddleHandler.onPaddleMove = (position) => connection.UpdatePosition(gameCode, position);
  renderer.showPaddle();
};

menuHandler.onNewGameClicked = () => {
  connection.CreateGame();
  connection.onCodeSet = (code) => {
    gameCode = code;
    connection.onGameStateUpdate = (state) => canvasEngine.drawFrame(state);
    renderer.showGame();
  };
};

renderer.showMenu();