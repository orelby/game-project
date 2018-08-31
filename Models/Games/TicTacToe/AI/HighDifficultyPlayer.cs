using System;

namespace WebAPI.Models.Games.TicTacToe.AI
{
    public class HighDifficultyPlayer : BaseMinimaxPlayer
    {
        public HighDifficultyPlayer(string id)
        : base(id)
        { }

        public override byte NextMove(GameManager gameManager) {
            return this.NextMove(gameManager, 4);
        }
    }
}
