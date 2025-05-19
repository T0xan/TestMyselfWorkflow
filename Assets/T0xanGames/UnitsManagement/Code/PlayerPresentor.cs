using System;
using UnityEngine;

namespace T0xanGames.UnitsManagement
{
    public class PlayerPresentor : MonoBehaviour
    {
        private PlayerBase _target;

        private PlayerUnitsManagment _playerUnitManagment;
        private PlayerScreenSpaceSelector _playerScreenSpaceSelector;

        public event Action<PlayerBase> OnTargetReassigned;

        #region Player Events
        public event Action<ISelectable> OnSelect;
        public event Action<ISelectable> OnDeselect;
        public void event_OnSelect(ISelectable x) => OnSelect?.Invoke(x);
        public void event_OnDeselect(ISelectable x) => OnDeselect?.Invoke(x);

        public event Action<FollowPoint> OnFollowPointSetted;
        public void event_OnFollowPointSetted(FollowPoint x) => OnFollowPointSetted?.Invoke(x);
        #endregion

        public PlayerUnitsManagment PlayerUnitsManagment => _playerUnitManagment;
        public PlayerScreenSpaceSelector PlayerScreenSpaceSelector => _playerScreenSpaceSelector;

        private void Start()
        {
            SetTarget(FindLocalPlayer());
        }

        public void SetTarget(PlayerBase target)
        {
            if (_target != null)
            {
                Describe();
            }

            _target = target;
            Subscribe();

            UpdatePlayerComponents();

            OnTargetReassigned?.Invoke(target);
        }

        private void Subscribe()
        {
            _target.OnActiveComponentsChanged += UpdatePlayerComponents;
        }
        private void Describe()
        {
            _target.OnActiveComponentsChanged += UpdatePlayerComponents;
        }

        private void UpdatePlayerComponents()
        {
            FindPlayerComponent(ref _playerScreenSpaceSelector, x =>
            {
                x.OnSelect += event_OnSelect;
                x.OnDeselect += event_OnDeselect;
            }, x =>
            {
                x.OnSelect -= event_OnSelect;
                x.OnDeselect -= event_OnDeselect;
            });
            FindPlayerComponent(ref _playerUnitManagment, x =>
            {
                x.OnFollowPointSetted += event_OnFollowPointSetted;
            }, x =>
            {
                x.OnFollowPointSetted -= event_OnFollowPointSetted;
            });
        }
        private void FindPlayerComponent<T>(ref T target, Action<T> subscribe = null, Action<T> describe = null) where T : PlayerComponent
        {
            var founded = _target.FindComponent<T>();

            if (target != founded)
            {
                if (target != null)
                    describe?.Invoke(target);
                if (founded != null)
                    subscribe?.Invoke(founded);
            }

            target = founded;
        }

        private PlayerBase FindLocalPlayer()
        {
            return PlayerBase.LocalPlayer;
        }
    }
}
