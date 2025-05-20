using System;
using UnityEngine;

namespace T0xanGames.UnitsManagement
{
    public class PlayerUnitsManagment : PlayerComponent
    {
        [Header("References:")]
        [SerializeField]
        private Component m_SelectorComponent;

        private ISelector _selector;
        private ISelectable[] _totalUnits;

        public event Action<FollowPoint> OnFollowPointSetted;

        private void Awake()
        {
            _totalUnits = FindObjectsByType<UnitBehaviour>(FindObjectsSortMode.None);

            _selector = m_SelectorComponent as ISelector;
            if (_selector is null)
                throw new UnassignedReferenceException();
            _selector.Initialize();
        }

        private void LateUpdate()
        {
            if (Base.Input.LeftMouseDown)
                _selector.StartSelect();
            else if (Base.Input.LeftMouse)
                _selector.UpdateSelecting(GetUnits());
            else if (Base.Input.LeftMouseUp)
                _selector.EndSelect();

            if (Base.Input.SpawnFollowPoint)
                SetFollowPoint();
        }

        private void SetFollowPoint()
        {
            Vector3? position = _selector.SelectWorldPoint();
            if (!position.HasValue)
                return;

            FollowPoint point = new FollowPoint(position.Value);

            foreach (var unit in _selector.GetSelected())
            {
                if (unit is ISubordinate subordinate)
                {
                    subordinate.SetTargetPoint(this, point);
                }
            }

            OnFollowPointSetted?.Invoke(point);
        }

        private ISelectable[] GetUnits()
        {
            return _totalUnits;
        }
    }

    public class FollowPoint
    {
        private Vector3 _worldPosition;

        public Vector3 Position => _worldPosition;

        public FollowPoint(Vector3 worldPosition)
        {
            _worldPosition = worldPosition;
        }
    }
}
