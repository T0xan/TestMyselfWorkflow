using UnityEngine;

namespace T0xanGames.UnitsManagement
{
    public class ScreenSpaceSelectionDrawer : MonoBehaviour
    {
        [SerializeField]
        private PlayerPresentor m_Presentor;

        [Header("References:")]
        [SerializeField]
        private RectTransform m_SelectionArea;

        private void Awake()
        {
            m_SelectionArea.pivot = new Vector2(0.5f, 0.5f);
        }

        private void Update()
        {
            Draw();
        }

        private void Draw()
        {
            var component = m_Presentor.PlayerScreenSpaceSelector;

            if (component == null || !component.IsSelecting)
            {
                m_SelectionArea.gameObject.SetActive(false);
                return;
            }

            m_SelectionArea.gameObject.SetActive(true);

            Vector2 areaSize = new Vector2(Mathf.Abs(component.SelectionRect.size.x), Mathf.Abs(component.SelectionRect.size.y));

            m_SelectionArea.position = component.SelectionRect.center;
            m_SelectionArea.sizeDelta = areaSize;
        }
    }
}
