using System;

using WebAPI.Models;
using WebAPI.Models.Games.TicTacToe;

namespace WebAPI.Hubs.TicTacToe
{
    public class PlayerState
    {
        public bool InQueue { get; set; }
        public IPlayer Player { get; private set; }
        public IGameManager Game { get; set; }
        public object SyncRoot { get { return syncRoot; } }

        private readonly object syncRoot = new object();

        public PlayerState(IPlayer player, object syncRoot = null)
        {
            this.Player = player;
            
            if (syncRoot != null)
            {
                this.syncRoot = syncRoot;
            }
        }
    }
}
