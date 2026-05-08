using System;
using System.Collections.Generic;

namespace PocketDarkSouls
{

    public sealed class EntityEvents
    {
        private readonly Dictionary<Type, List<Delegate>> _listeners = new Dictionary<Type, List<Delegate>>();

        // ------------------------------------------------
        // SUBSCRIBE / UNSUBSCRIBE
        // ------------------------------------------------

        public void Subscribe<T>(Action<T> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler)); 

            Type eventType = typeof(T); 

            if (!_listeners.TryGetValue(eventType, out List<Delegate>? handlers)) 
            {
                handlers                = new List<Delegate>();
                _listeners[eventType]   = handlers;
            } 

            handlers.Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            if (handler == null) 
                throw new ArgumentNullException(nameof(handler));

            Type eventType = typeof(T); 

            if (!_listeners.TryGetValue(eventType, out List<Delegate>? handlers)) 
                return;

            handlers.Remove(handler); 

            if (handlers.Count == 0)
            {
                _listeners.Remove(eventType);
            } 
        }

        // ------------------------------------------------
        // RAISE EVENT
        // ------------------------------------------------

        public void Raise<T>(T eventData) 
        {
            Type eventType  = typeof(T); 

            if (!_listeners.TryGetValue(eventType, out List<Delegate>? handlers))
                return;
              
            // Prevent modification during iteration
            Delegate[] invokeList = handlers.ToArray();

            for (int i = 0; i < invokeList.Length; i++)
            {
                ((Action<T>)invokeList[i]).Invoke(eventData); 
            }
        }

        // ------------------------------------------------
        // HELPERS
        // ------------------------------------------------

        public bool HasListeners<T>()
        {
            return _listeners.ContainsKey(typeof(T));
        }

        public void Clear<T>()
        {
            _listeners.Remove(typeof(T));
        }

        public void ClearAll()
        {  
             _listeners.Clear();  
        }
    }

    // ------------------------------------------------
    // EVENTS 
    // ------------------------------------------------


    public sealed class DeathEvent
    {
        public string EntityName { get; } 

        public DeathEvent(string entityName)
        {
            EntityName = entityName;  
        }
    }


}