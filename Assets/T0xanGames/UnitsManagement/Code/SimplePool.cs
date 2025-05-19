using System;
using System.Collections.Generic;
using UnityEngine;

namespace T0xanGames.UnitsManagement
{
    public class SimplePool<T> where T : UnityEngine.Object
    {
        private readonly T _prefab;

        private readonly Queue<T> _queue;
        private readonly List<T> _active;

        private readonly Action<T> _takeAction;
        private readonly Action<T> _returnAction;

        public SimplePool(T prefab, int preloadCount, Action<T> takeAction, Action<T> returnAction)
        {
            _prefab = prefab;

            _queue = new Queue<T>();
            _active = new List<T>();

            _takeAction = takeAction;
            _returnAction = returnAction;

            for (int i = 0; i < preloadCount; i++)
            {
                Return(CreateInstance());
            }
        }

        private T CreateInstance()
        {
            T instance = GameObject.Instantiate(_prefab);
            
            return instance;
        }

        public T Take()
        {
            T instance = _queue.Count > 0 ? _queue.Dequeue() : CreateInstance();
            _active.Add(instance);

            _takeAction(instance);

            return instance;
        }
        public void Return(T instance)
        {
            _active.Remove(instance);
            _returnAction(instance);
            _queue.Enqueue(instance);
        }
        public void ReturnAll()
        {
            foreach (var item in _active.ToArray())
                Return(item);
        }
    }
}
