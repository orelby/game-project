
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class UserPoolMapping<TId, TState>
    {

        private readonly object syncRoot = new object();

        private readonly Dictionary<TId, TState> _states =
            new Dictionary<TId, TState>();

        public UserPoolMapping(object syncRoot = null)
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
                return _states.Count;
            }
        }

        public void Set(TId id, TState state)
        {
            lock (_states)
            {
                if (!_states.ContainsKey(id))
                {
                    _states.Add(id, state);
                }
                else
                {
                    _states[id] = state;
                }
            }
        }

        public bool TryGet(TId id, out TState state)
        {
            TState _state;
            if (_states.TryGetValue(id, out _state))
            {
                state = _state;
                return true;
            }

            state = default(TState);
            return false;
        }

        public void Remove(TId id)
        {
            TState _state;
            if (!_states.TryGetValue(id, out _state))
            {
                return;
            }

            lock (_states)
            {
                _states.Remove(id);
            }
        }
    }
}
