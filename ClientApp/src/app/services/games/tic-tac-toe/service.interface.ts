import { TicTacToeGameHandlerInterface } from '../../../models/games/tic-tac-toe/handler.interface';

export interface TicTacToeGameServiceInterface {

    initGame(gameHandler: TicTacToeGameHandlerInterface): void;

}
