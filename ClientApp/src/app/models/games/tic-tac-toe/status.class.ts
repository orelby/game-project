import { TicTacToeGameType } from './type.enum';
import { TicTacToeAiGameDifficulty } from './ai-difficulty.enum';

export class TicTacToeGameStatus {
    gameType: TicTacToeGameType | null = null;
    aiDifficulty: TicTacToeAiGameDifficulty | null = null;
    aiFirstTurn = 0;
    hasGameStarted = false;
    isWaitingForOpponent = false;
    isWaitingForPlayer = false;
    isSendingMove = false;
    hasOpponentLeft = false;
    gameResult: number | null = null;

    constructor() {}

    reset() {
        this.gameType = null;
        this.aiDifficulty = null;
        this.aiFirstTurn = 0;
        this.hasGameStarted = false;
        this.isWaitingForOpponent = false;
        this.isWaitingForPlayer = false;
        this.isSendingMove = false;
        this.hasOpponentLeft = false;
        this.gameResult = null;
    }
}
