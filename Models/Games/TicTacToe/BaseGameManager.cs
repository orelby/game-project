using System.Collections.Generic;
using System.Linq;

using WebAPI.Models.Games.TicTacToe;

namespace WebAPI.Models.Games.TicTacToe
{
    public abstract class BaseGameManager : IGameManager
    {
        protected Board board;
        protected IPlayer[] players;

        public bool IsGameOver { get; private set; }
        public IPlayer Winner { get; private set; }

        public BaseGameManager(IPlayer player1, IPlayer player2)
        {
            this.board = new Board();
            this.players = new IPlayer[] { player1, player2 };
        }

        public bool TryNewMove(byte cell, IPlayer player)
        {
            if (!board.CellExists(cell)
                || board.GetCellOwner(cell) != null)
            {
                return false;
            }

            board.SetCellOwner(cell, player);
            checkGameStatus();

            return true;
        }

        public void NewMove(byte cell, IPlayer player)
        {
            board.SetCellOwner(cell, player);
            checkGameStatus();
        }

        private void checkGameStatus()
        {
            bool anyMovesLeft = this.GetRemainingMoves().Count > 0;
            IPlayer winner = null;

            foreach (List<byte> path in board.GetAllPaths())
            {
                var counter = new Dictionary<IPlayer, byte>() {
                    { players[0], 0 },
                    { players[1], 0 }
                };

                foreach (byte cell in path)
                {
                    var player = board.GetCellOwner(cell);

                    if (player != null)
                    {
                        counter[player]++;
                    }
                }

                if (counter.Values.Min() == 0)
                {
                    byte maxCount = counter.Values.Max();
                    if (maxCount == Board.GridSize)
                    {
                        winner = counter
                            .Where(count => count.Value == maxCount)
                            .Single()
                            .Key;
                    }
                }
            }

            if (winner != null
                || !anyMovesLeft)
            {
                IsGameOver = true;
                Winner = winner;
            }
            else
            {
                // reset in case AI messed with it
                IsGameOver = false;
                Winner = null;
            }
        }

        public List<byte> GetRemainingMoves()
        {
            return this.board.GetAllCells()
                .Select((player, i) => new { Index = i, Player = player })
                .Where(el => el.Player == null)
                .Select(el => (byte)el.Index)
                .ToList();
        }

        public IPlayer GetOpponent(IPlayer player)
        {
            if (player == players[0])
            {
                return players[1];
            }

            return players[0];
        }

        public Movement MovementFromCell(byte cell)
        {
            return Board.MovementFromCell(cell);
        }

        public List<List<byte>> GetContainingPaths(byte cell)
        {
            return this.board.GetAllPaths().Where(path => path.Contains(cell)).ToList();
        }

        public IPlayer GetCellOwner(byte cell)
        {
            return this.board.GetCellOwner(cell);
        }
    }
}
