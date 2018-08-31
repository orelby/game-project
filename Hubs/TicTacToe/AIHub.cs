using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

using WebAPI.Models;
using WebAPI.Models.Games.TicTacToe;
using WebAPI.Models.Games.TicTacToe.AI;

namespace WebAPI.Hubs.TicTacToe
{
    public class AIHub : Hub<IPlayerProxy>
    {
        private readonly static object _syncRoot = new object();
        private readonly static Dictionary<string, PlayerState> players =
            new Dictionary<string, PlayerState>();
        public async void StartGame(AIGameDifficulty difficulty, Boolean isPlayerFirstToPlay)
        {
            PlayerState playerState = null;

            lock (_syncRoot)
            {
                playerState = new PlayerState(
                    new HumanPlayer(Context.ConnectionId),
                    new object()
                );

                players[Context.ConnectionId] = playerState;
            }

            lock (playerState.SyncRoot)
            {
                IAIPlayer aiPlayer;

                {
                    var aiPlayerId = playerState.Player.Id + "-ai";

                    switch (difficulty)
                    {
                        case AIGameDifficulty.Impossible:
                            aiPlayer = new ImpossibleDifficultyPlayer(aiPlayerId);
                            break;

                        case AIGameDifficulty.High:
                            aiPlayer = new HighDifficultyPlayer(aiPlayerId);
                            break;

                        default:
                            aiPlayer = new LowDifficultyPlayer(aiPlayerId);
                            break;
                    }
                }

                var game = new GameManager(playerState.Player, aiPlayer);
                playerState.Game = game;


                Clients.Caller
                    .OpponentFound()
                    .ContinueWith(x =>
                    {
                        Random rand = new Random();

                        if (isPlayerFirstToPlay)
                        {
                            Clients.Caller.PlayerTurn(null);

                        }
                        else
                        {
                            Clients.Caller.OpponentTurn();
                            var opponent = (IAIPlayer)game.GetOpponent(playerState.Player);

                            byte opponentMove = opponent.NextMove((GameManager)game);
                            game.TryNewMove(opponentMove, opponent);
                            Clients.Caller.PlayerTurn(game.MovementFromCell(opponentMove));
                        }
                    });
            }
        }

        public async Task NewMove(Movement move)
        {
            PlayerState playerState;
            if (players.TryGetValue(Context.ConnectionId, out playerState))
            {
                lock (playerState.SyncRoot)
                {
                    var game = playerState.Game;
                    byte cell = (byte)((move.row - 1) * 3 + move.col - 1);

                    var opponent = (IAIPlayer)game.GetOpponent(playerState.Player);
                    if (!game.TryNewMove(cell, playerState.Player))
                    {
                        Clients.Caller.InvalidMove();
                        return;
                    }

                    if (game.IsGameOver)
                    {
                        if (game.Winner == null)
                        {
                            Clients.Caller.GameFinished(0);
                        }
                        else
                        {
                            Clients.Caller.GameFinished((sbyte)(game.Winner == playerState.Player ? 1 : -1));
                        }
                    }
                    else
                    {
                        // AI turn
                        Clients.Caller.OpponentTurn();
                        byte opponentMove = opponent.NextMove((GameManager)game);
                        game.TryNewMove(opponentMove, opponent);

                        if (game.IsGameOver)
                        {
                            if (game.Winner == null)
                            {
                                Clients.Caller.GameFinished(0, game.MovementFromCell(opponentMove));
                            }
                            else
                            {
                                Clients.Caller.GameFinished((sbyte)(game.Winner == playerState.Player ? 1 : -1), game.MovementFromCell(opponentMove));
                            }
                        }
                        else
                        {
                            Clients.Caller.PlayerTurn(game.MovementFromCell(opponentMove));
                        }
                    }
                }
            }
        }

        public async Task RestartGame()
        {
            throw new NotImplementedException();
        }
    }

    public enum AIGameDifficulty
    {
        Low = 5,
        High = 10,
        Impossible = 15
    }
}
