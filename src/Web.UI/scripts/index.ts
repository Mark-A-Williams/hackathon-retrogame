import '../styles/main.css';
import * as signalR from '@aspnet/signalr';
import { CanvasEngine } from './canvasEngine';
import { GameState } from './models';

const divMessages: HTMLDivElement = document.querySelector('#divMessages');
const tbMessage: HTMLInputElement = document.querySelector('#tbMessage');
const btnSend: HTMLButtonElement = document.querySelector('#btnSend');
const btnFrame: HTMLButtonElement = document.querySelector('#frameButton');
const username = new Date().getTime();
const canvasEngine = new CanvasEngine();

const connection = new signalR.HubConnectionBuilder().withUrl('/hub').build();

connection.on('messageReceived', (username: string, message: string) => {
  let messageContainer = document.createElement('div');

  messageContainer.innerHTML = `<div class="message-author">${username}</div><div>${message}</div>`;

  divMessages.appendChild(messageContainer);
  divMessages.scrollTop = divMessages.scrollHeight;
});

connection.start().catch(err => document.write(err));

tbMessage.addEventListener('keyup', (e: KeyboardEvent) => {
  if (e.keyCode === 13) {
    send();
  }
});

btnSend.addEventListener('click', send);
btnFrame.addEventListener('click', () => {
  const gameState = canvasEngine.createMockGamestate(5);
  canvasEngine.drawFrame(gameState);
});
function send() {
  connection
    .send('newMessage', username, tbMessage.value)
    .then(() => (tbMessage.value = ''));
}
