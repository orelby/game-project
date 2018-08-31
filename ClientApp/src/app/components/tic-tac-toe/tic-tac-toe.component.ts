import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Observable } from 'rxjs';

import { TicTacToeGameHandlerInterface } from '../../models/games/tic-tac-toe/handler.interface';
import { TicTacToeGameMovementInterface } from '../../models/games/tic-tac-toe/movement.interface';
import { PlayerInterface } from '../../models/games/player.interface';

import { TicTacToeGameBoard } from '../../models/games/tic-tac-toe/board.class';
import { TicTacToeGameType } from '../../models/games/tic-tac-toe/type.enum';
import { TicTacToeGameStatus } from '../../models/games/tic-tac-toe/status.class';
import { TicTacToeGameServiceInterface } from '../../services/games/tic-tac-toe/service.interface';
import { TicTacToeAiService } from '../../services/games/tic-tac-toe/ai.service';
import { TicTacToePvpService } from '../../services/games/tic-tac-toe/pvp.service';
import { TicTacToeAiGameDifficulty } from '../../models/games/tic-tac-toe/ai-difficulty.enum';

@Component({
  selector: 'app-tic-tac-toe',
  templateUrl: './tic-tac-toe.component.html',
  styleUrls: ['./tic-tac-toe.component.scss']
})
export class TicTacToeComponent implements OnInit, TicTacToeGameHandlerInterface {

  gameTypes = TicTacToeGameType;
  aiDifficulties = TicTacToeAiGameDifficulty;

  /* called to resolve a promise */
  onMovement?: (movement: TicTacToeGameMovementInterface) => void;

  player: PlayerInterface | null = null;
  opponent: PlayerInterface | null = null;
  gameService?: TicTacToeGameServiceInterface;

  board?: TicTacToeGameBoard;
  status: TicTacToeGameStatus;
  needsToChooseAiOptions = false;
  aiDifficulty: TicTacToeAiGameDifficulty = TicTacToeAiGameDifficulty.Impossible;
  aiFirstTurn = 0;

  constructor(private route: ActivatedRoute) {
    this.status = new TicTacToeGameStatus();
  }

  ngOnInit() { }

  reset() {
    this.status.reset();
    this.status.gameType = null;
    this.status.aiDifficulty = null;
    this.board = new TicTacToeGameBoard();
    this.needsToChooseAiOptions = false;
  }


  startAiGame(difficulty: TicTacToeAiGameDifficulty, aiFirstTurn: number) {
    this.reset();
    this.status.gameType = this.gameTypes.AI;
    this.status.aiDifficulty = difficulty;
    this.status.aiFirstTurn = aiFirstTurn;

    let isPlayerFirstToPlay: boolean;
    switch (aiFirstTurn) {
      case 1:
        isPlayerFirstToPlay = true;
        break;

      case 0:
        if (Math.random() * 2 > 1) {
          isPlayerFirstToPlay = true;
        } else {
          isPlayerFirstToPlay = false;
        }
        break;

      default:
        isPlayerFirstToPlay = false;
        break;
    }

    this.gameService = new TicTacToeAiService(difficulty, isPlayerFirstToPlay);
    this.player = { name: 'Me', isHuman: true };

    this.gameService.initGame(this);
  }

  startPvPGame() {
    this.reset();
    this.status.gameType = this.gameTypes.PvP;

    this.gameService = new TicTacToePvpService();
    this.player = { name: 'Me', isHuman: true };
    this.opponent = { name: 'Opponent', isHuman: true };

    this.gameService.initGame(this);
  }

  makeMove(cell: number) {
    if (!this.board) {
      throw new Error('Cannot execute game movement because no board is set.');
    }

    if (!this.onMovement) {
      throw new Error('Cannot execute game movement because it is not expected.');
    }

    if (this.status.isSendingMove || !this.status.isWaitingForPlayer) {
      return false;
    }

    this.board.setCellOwner(cell, this.player);
    this.status.isSendingMove = true;
    this.status.isWaitingForPlayer = false;

    this.onMovement(this.board.getGameMovementFromCell(cell));
  }


  onOpponentFound(opponent: PlayerInterface) {
    this.opponent = opponent;
    this.status.hasGameStarted = true;
  }

  onOpponentLeft() {
    this.status.isWaitingForPlayer = false;
    this.status.isWaitingForOpponent = false;
    this.status.hasOpponentLeft = true;
  }

  onInvalidMove() {
    console.log('Oh snap, what a hacker!');
  }

  onOpponentTurn() {
    this.status.isSendingMove = false;
    this.status.isWaitingForOpponent = true;
  }

  onPlayerTurn(opponentMovement?: TicTacToeGameMovementInterface) {
    if (!this.board) {
      throw new Error('Cannot continue without a game board');
    }

    if (opponentMovement) {
      this.board.setCellOwner(opponentMovement, this.opponent);
    }

    this.status.isWaitingForOpponent = false;
    this.status.isWaitingForPlayer = true;

    return new Promise<TicTacToeGameMovementInterface>(((resolve: any) => {
      this.onMovement = resolve;
    }).bind(this));
  }

  onGameFinished(result: number, opponentMovement?: TicTacToeGameMovementInterface) {
    this.status.isSendingMove = false;
    this.status.isWaitingForOpponent = false;
    this.status.isWaitingForPlayer = false;

    this.status.gameResult = result;

    if (!this.board) {
      throw new Error('Cannot continue without a game board');
    }

    if (opponentMovement) {
      this.board.setCellOwner(opponentMovement, this.opponent);
    }

    // TODO: allow restart
  }

  onGameResrarted() {
    // TODO
    this.status.gameResult = null;
  }

  restartGame() {
    if (this.status.gameType === this.gameTypes.AI) {
      if (this.status.aiDifficulty != null) {
        this.startAiGame(this.status.aiDifficulty, this.status.aiFirstTurn);
      }
    } else {
      this.startPvPGame();
    }
  }
}
