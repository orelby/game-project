using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

using WebAPI.Models;
using WebAPI.Models.Games.TicTacToe;
using WebAPI.Models.Games.TicTacToe.PvP;

namespace WebAPI.Hubs.TicTacToe
{
    public class PvPHub : Hub<IPlayerProxy>
    {
        private readonly static object _syncRoot = new object();
        private readonly static Dictionary<string, PlayerState> players =
            new Dictionary<string, PlayerState>();
        public async void StartGame()
        {
            PlayerState opponentState = null;
            PlayerState playerState = null;

            lock (_syncRoot)
            {
                var possibleOpponent = players.Where(state => state.Value.InQueue).FirstOrDefault();
                if (possibleOpponent.Key != default(string))
                {
                    opponentState = possibleOpponent.Value;
                }

                playerState = new PlayerState(
                    new HumanPlayer(Context.ConnectionId),
                    (opponentState == null) ? new object() : opponentState.SyncRoot
                );

                players[Context.ConnectionId] = playerState;

                if (opponentState == null)
                {
                    playerState.InQueue = true;

                    return;
                }

                opponentState.InQueue = false;
            }

            lock (playerState.SyncRoot)
            {
                var gameManager = new GameManager(playerState.Player, opponentState.Player);
                playerState.Game = gameManager;
                opponentState.Game = gameManager;

                var opponent = Clients.Client(opponentState.Player.Id);

                opponent
                    .OpponentFound()
                    .ContinueWith(x => opponent.PlayerTurn(null));

                Clients.Caller
                    .OpponentFound()
                    .ContinueWith(x => Clients.Caller.OpponentTurn());
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
                    var cell = (byte)((move.row - 1) * 3 + move.col - 1);

                    var opponent = game.GetOpponent(playerState.Player);
                    PlayerState opponentState;
                    if (players.TryGetValue(opponent.Id, out opponentState))
                    {
                        var opponentProxy = Clients.Client(opponentState.Player.Id);

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
                                opponentProxy.GameFinished(0, move);
                            }
                            else
                            {
                                Clients.Caller.GameFinished((sbyte)(game.Winner == playerState.Player ? 1 : -1));
                                opponentProxy.GameFinished((sbyte)(game.Winner == opponent ? 1 : -1), move);
                            }
                        }
                        else
                        {
                            Clients.Caller.OpponentTurn();
                            opponentProxy.PlayerTurn(move);
                        }
                    }
                }
            }
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                lock (_syncRoot)
                {
                    PlayerState playerState;
                    if (players.TryGetValue(Context.ConnectionId, out playerState))
                    {
                        lock (playerState.SyncRoot)
                        {
                            if (playerState.Game != null)
                            {
                                var opponent = playerState.Game.GetOpponent(playerState.Player);
                                var opponentProxy = Clients.Client(opponent.Id);
                                opponentProxy.OpponentLeft();

                                PlayerState opponentState;
                                if (players.TryGetValue(opponent.Id, out opponentState)) {
                                    opponentState.Game = null;
                                }
                            }

                            players.Remove(Context.ConnectionId);
                        }
                    }
                }
            }
            catch (Exception e) { }
        }
        public async Task RestartGame()
        {

        }
    }
}
