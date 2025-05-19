using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace T0xanGames.UnitsManagement
{
    public class UnitEyes : MonoBehaviour
    {
        [SerializeField]
        private UnitBehaviour m_Unit;

        [Header("References:")]
        [SerializeField]
        private Transform[] m_Eyes;
        [SerializeField]
        private float m_SmoothTime;

        [Space]
        [SerializeField]
        private Vector3 m_Offset;

        private bool _lookAtTarget;

        private void OnEnable()
        {
            m_Unit.OnTargetPointSetted += OnTargetPointSetted;
        }
        private void OnDisable()
        {
            m_Unit.OnTargetPointSetted -= OnTargetPointSetted;
        }

        private void Update()
        {
            Look();
        }

        private void Look()
        {
            foreach (Transform eye in m_Eyes)
            {
                eye.rotation = Quaternion.Slerp(eye.rotation, GetTargetRotation(eye), m_SmoothTime * Time.deltaTime);
            }
        }

        private Quaternion GetTargetRotation(Transform eye)
        {
            if (_lookAtTarget)
            {
                return Quaternion.LookRotation(m_Unit.Point.Position + m_Offset - eye.position, Vector3.up);
            }

            return m_Unit.transform.rotation;
        }

        private void OnTargetPointSetted(FollowPoint point)
        {
            _lookAtTarget = point != null;
        }
    }
}
