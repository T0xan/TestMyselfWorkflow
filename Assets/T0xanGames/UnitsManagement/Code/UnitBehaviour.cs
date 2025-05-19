using System;
using UnityEngine;
using UnityEngine.AI;

namespace T0xanGames.UnitsManagement
{
    public class UnitBehaviour : MonoBehaviour, ISelectable, ISubordinate
    {
        [Header("References:")]
        [SerializeField]
        private NavMeshAgent m_Agent;

        [Space]
        [SerializeField]
        private Vector3 m_Size;
        [SerializeField]
        private Vector3 m_Offset;

        private bool _isSelected;
        private FollowPoint _point;

        public event Action OnSelect;
        public event Action OnDeselect;
        public event Action<FollowPoint> OnTargetPointSetted;

        public bool IsSelected => _isSelected;
        public FollowPoint Point => _point;

        public void Select()
        {
            _isSelected = true;

            OnSelect?.Invoke();
        }
        public void Deselect()
        {
            _isSelected = false;

            OnDeselect?.Invoke();
        }

        private void Update()
        {
            if (_point != null)
            {
                m_Agent.isStopped = false;
                m_Agent.SetDestination(_point.Position);
            }
            else
            {
                m_Agent.isStopped = true;
            }
        }

        public Bounds GetWorldSpaceBounds()
        {
            return new Bounds(transform.position + m_Offset, m_Size);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Bounds bounds = GetWorldSpaceBounds();
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }

        public FollowPoint GetTargetPoint()
        {
            return _point;
        }

        public void SetTargetPoint(FollowPoint point)
        {
            _point = point;
            OnTargetPointSetted?.Invoke(point);
        }
    }
}
