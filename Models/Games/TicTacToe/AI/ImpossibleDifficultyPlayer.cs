using System;

namespace WebAPI.Models.Games.TicTacToe.AI
{
    public class ImpossibleDifficultyPlayer : BaseMinimaxPlayer
    {
        public ImpossibleDifficultyPlayer(string id)
        : base(id)
        { }

        public override byte NextMove(GameManager gameManager) {
            return this.NextMove(gameManager, 6);
        }
    }
}
