using System;
using System.Collections.Generic;

namespace WebAPI.Models.Games.TicTacToe
{
    public class Board
    {
        public const byte GridSize = 3;

        private IPlayer[] cells;
        private List<List<byte>> allPaths;

        public Board()
        {
            cells = new IPlayer[GridSize * GridSize];
        }

        public IPlayer GetCellOwner(byte cell)
        {
            if (!CellExists(cell))
            {
                throw new ArgumentException("Invalid cell number");
            }

            return this.cells[cell];
        }

        public void SetCellOwner(byte cell, IPlayer player)
        {
            if (!CellExists(cell))
            {
                throw new ArgumentException("Invalid cell number");
            }

            this.cells[cell] = player;
        }

        public bool CellExists(byte cell)
        {
            if (cell >= 0 && cell < GridSize * GridSize)
            {
                return true;
            }

            return false;
        }

        public List<List<byte>> GetAllPaths()
        {
            if (this.allPaths != null)
            {
                return this.allPaths;
            }

            List<List<byte>> allPaths = new List<List<byte>>();

            // calculate rows and cols
            for (byte i = 0; i < GridSize; i++)
            {
                var rowPath = new List<byte>();
                var colPath = new List<byte>();

                for (byte j = 0; j < GridSize; j++)
                {
                    rowPath.Add((byte)(j + i * GridSize));
                    colPath.Add((byte)(i + j * GridSize));
                }

                allPaths.Add(rowPath);
                allPaths.Add(colPath);
            }

            // calculate diagonals
            var firstDiagonal = new List<byte>();
            var secondDiagonal = new List<byte>();
            var cellUpperLimit = GridSize * GridSize;

            for (byte i = 0, j = GridSize - 1;
                i < cellUpperLimit && j < cellUpperLimit - 1;
                i += GridSize + 1, j += GridSize - 1)
            {
                firstDiagonal.Add(i);
                secondDiagonal.Add(j);
            }

            allPaths.Add(firstDiagonal);
            allPaths.Add(secondDiagonal);

            // end
            return (this.allPaths = allPaths);
        }

        public IPlayer[] GetAllCells()
        {
            return this.cells;
        }

        public static Movement MovementFromCell(byte cell)
        {
            return new Movement()
            {
                row = (cell / GridSize) + 1,
                col = (cell % GridSize) + 1
            };
        }
    }
}
