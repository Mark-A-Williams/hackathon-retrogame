import '../styles/main.css';
import * as signalR from '@aspnet/signalr';
import { CanvasEngine } from './canvasEngine';
import { GameState } from './models';

const btnFrame: HTMLButtonElement = document.querySelector("#frameButton");
const canvasEngine = new CanvasEngine();

const createGameBtn: HTMLButtonElement = document.querySelector("#create-game-button");
const createdGameCodeOuput: HTMLParagraphElement = document.querySelector("#created-game-code");
const joinedPlayersOutput: HTMLUListElement = document.querySelector("#joined-players");

const usernameInput: HTMLInputElement = document.querySelector('#username-input');
const gameCodeInput: HTMLInputElement = document.querySelector('#game-code-input');
const joinGameBtn: HTMLButtonElement = document.querySelector('#join-game-button');

const connection = new signalR.HubConnectionBuilder().withUrl('/hub').build();

connection.on("onCodeSet", (code: string) => {
    createdGameCodeOuput.innerText = code;
});

connection.on("onPlayerJoined", (userName: string) => {
    const newListEl = document.createElement('li');
    newListEl.innerText = userName;
    joinedPlayersOutput.appendChild(newListEl);
});

createGameBtn.addEventListener("click", () => connection.invoke("CreateNewGame"));
joinGameBtn.addEventListener("click", () => connection.invoke("JoinGame", gameCodeInput.value, usernameInput.value));

connection.start().catch(err => document.write(err));
btnFrame.addEventListener("click", () => canvasEngine.drawFrame(null));
