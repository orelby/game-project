import { Injectable } from '@angular/core';
import { TicTacToeGameHandlerInterface } from '../../../models/games/tic-tac-toe/handler.interface';
import { TicTacToeGameServiceInterface } from './service.interface';
import { HubConnection, LogLevel, HubConnectionBuilder } from '../../../../../node_modules/@aspnet/signalr';
import { TicTacToeGameMovementInterface } from '../../../models/games/tic-tac-toe/movement.interface';

export class TicTacToePvpService implements TicTacToeGameServiceInterface {
  connection: HubConnection;
  handler: TicTacToeGameHandlerInterface | null = null;
  onStart: Promise<void>;

  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl('/signalr/games/tic-tac-toe/pvp-hub')
      // .withUrl('http://localhost:5000/signalr/games/tic-tac-toe/pvp-hub')
      .configureLogging(LogLevel.Debug)
      .build();

    this.connection.on('OpponentFound', (name) => {
      if (this.handler) {
        this.handler.onOpponentFound({ name: 'AI - Impossible', isHuman: false });
      }
    });

    this.connection.on('OpponentLeft', () => {
      if (this.handler) {
        this.handler.onOpponentLeft();
      }
    });

    this.connection.on('InvalidMove', () => {
      if (this.handler) {
        this.handler.onInvalidMove();
      }
    });

    this.connection.on('OpponentTurn', () => {
      if (this.handler) {
        this.handler.onOpponentTurn();
      }
    });

    this.connection.on('PlayerTurn', (opponentMovement?: TicTacToeGameMovementInterface) => {
      if (this.handler) {
        this.handler
          .onPlayerTurn(opponentMovement)
          .then((move: TicTacToeGameMovementInterface) => {
            this.connection.send('NewMove', move);
          });
      }
    });

    this.connection.on('GameFinished', (result: number, opponentMovement?: TicTacToeGameMovementInterface) => {
      if (this.handler) {
        this.handler.onGameFinished(result, opponentMovement);
      }
    });

    this.connection.on('GameRestarted', () => {
      if (this.handler) {
        this.handler.onGameResrarted();
      }
    });

    this.onStart = this.connection
      .start()
      .catch(err => console.error(err.toString()));
  }

  initGame(handler: TicTacToeGameHandlerInterface) {
    this.handler = handler;

    this.onStart.then(() => this.connection.send('StartGame'));
  }
}
