
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Models
{
    public class GamePoolMapping<TGameId, TUserId>
    {

        private readonly object syncRoot = new object();

        private readonly Dictionary<TGameId, HashSet<TUserId>> _users =
            new Dictionary<TGameId, HashSet<TUserId>>();

        public GamePoolMapping(object syncRoot = null)
        {
            if (syncRoot != null)
            {
                this.syncRoot = syncRoot;
            }
        }

        public int Count
        {
            get
            {
                return _users.Count;
            }
        }

        public void Add(TGameId key, TUserId userId)
        {
            lock (_users)
            {
                HashSet<TUserId> users;
                if (!_users.TryGetValue(key, out users))
                {
                    users = new HashSet<TUserId>();
                    _users.Add(key, users);
                }

                lock (users)
                {
                    users.Add(userId);
                }
            }
        }

        public IEnumerable<TUserId> GetUsers(TGameId key)
        {
            HashSet<TUserId> users;
            if (_users.TryGetValue(key, out users))
            {
                return users;
            }

            return Enumerable.Empty<TUserId>();
        }

        public void Remove(TGameId key, TUserId userId)
        {
            lock (_users)
            {
                HashSet<TUserId> users;
                if (!_users.TryGetValue(key, out users))
                {
                    return;
                }

                lock (users)
                {
                    _users.Remove(key);
                }
            }
        }
    }
}
