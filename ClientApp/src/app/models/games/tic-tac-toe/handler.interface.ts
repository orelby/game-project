import { TicTacToeGameMovementInterface } from './movement.interface';
import { PlayerInterface } from '../player.interface';

import { Observable } from 'rxjs';

export interface TicTacToeGameHandlerInterface {
    onOpponentFound(player: PlayerInterface): void;
    onOpponentLeft(): void;

    onInvalidMove(): void;
    onOpponentTurn(): void;
    onPlayerTurn(opponentMovement?: TicTacToeGameMovementInterface): Promise<TicTacToeGameMovementInterface>;

    onGameFinished(result: number, opponentMovement?: TicTacToeGameMovementInterface): void;
    onGameResrarted(): void;
}
