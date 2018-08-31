using System;

namespace WebAPI.Models.Games.TicTacToe.AI
{
    public class LowDifficultyPlayer : BaseMinimaxPlayer
    {
        public LowDifficultyPlayer(string id)
        : base(id)
        { }

        public override byte NextMove(GameManager gameManager) {
            return this.NextMove(gameManager, 2);
        }
    }
}
