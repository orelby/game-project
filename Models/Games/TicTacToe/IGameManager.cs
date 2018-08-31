namespace WebAPI.Models.Games.TicTacToe
{
    public interface IGameManager
    {

        bool IsGameOver { get; }
        IPlayer Winner { get; }

        bool TryNewMove(byte cell, IPlayer player);
        IPlayer GetOpponent(IPlayer player);

        Movement MovementFromCell(byte cell);
    }
}
