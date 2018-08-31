using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

using WebAPI.Models;
using WebAPI.Models.Games.TicTacToe;

namespace WebAPI.Models.Games.TicTacToe.AI
{
    public interface IAIPlayer : IPlayer
    {
        byte NextMove(GameManager gameManager);
    }
}
