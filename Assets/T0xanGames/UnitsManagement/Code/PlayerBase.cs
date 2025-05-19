using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace T0xanGames.UnitsManagement
{
    public class PlayerBase : MonoBehaviour
    {
        public static PlayerBase LocalPlayer { get; private set; }

        [SerializeField]
        private PlayerInput m_PlayerInput;

        private List<PlayerComponent> _components;

        public event Action OnActiveComponentsChanged;

        public PlayerInput Input => m_PlayerInput;

        private void Awake()
        {
            LocalPlayer = this;
        }

        public void AddComponent(PlayerComponent component)
        {
            if (_components == null)
                _components = new List<PlayerComponent>();

            _components.Add(component);
            OnActiveComponentsChanged?.Invoke();
        }
        public bool RemoveComponent(PlayerComponent component)
        {
            if (_components == null)
                return false;

            bool removed = _components.Remove(component);
            if (removed)
                OnActiveComponentsChanged?.Invoke();

            return removed;
        }

        public T FindComponent<T>() where T : PlayerComponent
        {
            return _components.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
        }
        public bool TryFindComponent<T>(out T component) where T : PlayerComponent
        {
            component = FindComponent<T>();
            return component != null;
        }
    }
}
