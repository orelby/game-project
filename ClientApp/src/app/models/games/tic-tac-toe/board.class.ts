import { PlayerInterface } from '../player.interface';
import { TicTacToeGameMovementInterface } from './movement.interface';

export class TicTacToeGameBoard {
    private cells: (PlayerInterface | null)[] = [];

    constructor() {
        for (let i = 0; i < 9; i++) {
            this.cells[i] = null;
        }
    }

    getCellOwner(cell: TicTacToeGameMovementInterface | number): PlayerInterface | null {
        if (typeof cell === 'number') {
            return this.cells[cell];
        }

        return this.cells[(cell.row - 1) * 3 + cell.col - 1];
    }

    setCellOwner(cell: TicTacToeGameMovementInterface | number, player: PlayerInterface | null) {
        if (typeof cell === 'number') {
            this.cells[cell] = player;
        } else {
            this.cells[(cell.row - 1) * 3 + cell.col - 1] = player;
        }
    }

    getGameMovementFromCell(cell: number): TicTacToeGameMovementInterface {
        const _row = Math.ceil((cell + 1) / 3);
        const _col = (cell % 3) + 1;
        // let _col = (cell + 1) % 3;
        // _col = (_col > 0) ? _col : 3;

        return { col: _col, row: _row };
    }
}
