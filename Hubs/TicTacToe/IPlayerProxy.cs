using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

using WebAPI.Models.Games.TicTacToe;

namespace WebAPI.Hubs.TicTacToe
{
    public interface IPlayerProxy : IClientProxy
    {
        Task OpponentFound();
        Task OpponentLeft();

        Task InvalidMove();
        Task OpponentTurn();
        Task PlayerTurn();
        Task PlayerTurn(Movement opponentMovement);

        Task GameFinished(sbyte result);
        Task GameFinished(sbyte result, Movement opponentMovement);
        Task GameRestarted();
    }
}
