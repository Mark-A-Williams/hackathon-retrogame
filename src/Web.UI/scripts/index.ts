import '../styles/main.css';
import * as signalR from '@aspnet/signalr';
import { CanvasEngine } from './canvasEngine';
import { GameState } from './models';
import { Connection } from './connection/connection-service';

const btnFrame: HTMLButtonElement = document.querySelector('#frameButton');
const canvasEngine = new CanvasEngine();

const createGameBtn: HTMLButtonElement = document.querySelector(
  '#create-game-button'
);
const createdGameCodeOuput: HTMLParagraphElement = document.querySelector(
  '#created-game-code'
);
const joinedPlayersOutput: HTMLUListElement = document.querySelector(
  '#joined-players'
);

const usernameInput: HTMLInputElement = document.querySelector(
  '#username-input'
);
const gameCodeInput: HTMLInputElement = document.querySelector(
  '#game-code-input'
);
const joinGameBtn: HTMLButtonElement = document.querySelector(
  '#join-game-button'
);
const touchControl: HTMLInputElement = document.querySelector('#touch-control');
const touchSlider: HTMLInputElement = document.querySelector('#touch-slider');
const connection = new Connection.ConnectionService();
let gameRoomName = "GAME ROOM";

connection.onCodeSet = (code: string) => {
  createdGameCodeOuput.innerText = code;
};

connection.onPlayerJoined = (userName: string) => {
  const newListEl = document.createElement('li');
  newListEl.innerText = userName;
  joinedPlayersOutput.appendChild(newListEl);
};

connection.onGameStateUpdate = (state: GameState) =>
  canvasEngine.drawFrame(state);

createGameBtn.addEventListener('click', () => connection.CreateGame());
joinGameBtn.addEventListener('click', () =>
  connection.JoinGame(usernameInput.value, gameCodeInput.value)
);

btnFrame.addEventListener('click', () => {
  const gameState = canvasEngine.createMockGamestate(5);
  canvasEngine.drawFrame(gameState);
});

touchSlider.oninput = () => {
  connection.UpdatePosition(this.gameRoomName, parseInt(touchSlider.value));
};
