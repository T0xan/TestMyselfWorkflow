using UnityEngine;

namespace T0xanGames.UnitsManagement
{
    public class UnitVisualizerExample : MonoBehaviour
    {
        [SerializeField]
        private UnitBehaviour m_Unit;

        [Header("Visualization:")]
        [SerializeField]
        private GameObject m_UpperRenderer;

        private void OnEnable()
        {
            m_Unit.OnSelect += UpdateVisual;
            m_Unit.OnDeselect += UpdateVisual;

            UpdateVisual();
        }
        private void OnDisable()
        {
            m_Unit.OnSelect -= UpdateVisual;
            m_Unit.OnDeselect -= UpdateVisual;
        }

        private void UpdateVisual()
        {
            m_UpperRenderer.SetActive(m_Unit.IsSelected);
        }
    }
}
