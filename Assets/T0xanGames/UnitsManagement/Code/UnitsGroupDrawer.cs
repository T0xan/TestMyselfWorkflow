using System.Collections.Generic;
using UnityEngine;

namespace T0xanGames.UnitsManagement
{
    public class UnitsGroupDrawer : MonoBehaviour
    {
        [SerializeField]
        private PlayerPresentor m_Presenter;

        [Header("Header:")]
        [SerializeField]
        private Transform m_MarkersContent;
        [SerializeField]
        private GameObject m_MarkerPrefab;
        [SerializeField]
        private int m_MarkerPreloadCount;

        private SimplePool<GameObject> _markersPool;

        private void Awake()
        {
            _markersPool = new SimplePool<GameObject>(m_MarkerPrefab, m_MarkerPreloadCount, TakeMarker, ReturnMarker);
        }

        private void OnEnable()
        {
            m_Presenter.OnSelect += UpdateSelected;
            m_Presenter.OnDeselect += UpdateSelected;
            m_Presenter.OnFollowPointSetted += FollowPointSetted;
        }

        private void OnDisable()
        {
            m_Presenter.OnSelect -= UpdateSelected;
            m_Presenter.OnDeselect -= UpdateSelected;
            m_Presenter.OnFollowPointSetted += FollowPointSetted;
        }

        private void TakeMarker(GameObject marker)
        {
            marker.SetActive(true);
        }
        private void ReturnMarker(GameObject marker)
        {
            marker.SetActive(false);
            marker.transform.SetParent(transform);
        }

        private void UpdateSelected(ISelectable selectable)
        {
            DrawPoints(m_Presenter.PlayerScreenSpaceSelector.GetSelected());
        }
        private void FollowPointSetted(FollowPoint point)
        {
            DrawPoints(m_Presenter.PlayerScreenSpaceSelector.GetSelected());
        }

        private void DrawPoints(ISelectable[] selected)
        {
            List<FollowPoint> points = new List<FollowPoint>();
            foreach (ISelectable item in selected)
            {
                if (item is ISubordinate subordinate)
                {
                    var point = subordinate.GetTargetPoint();
                    if (point != null && !points.Contains(point))
                    {
                        points.Add(point);
                    }
                }
            }

            _markersPool.ReturnAll();

            foreach (var point in points)
            {
                GameObject marker = _markersPool.Take();
                marker.transform.position = point.Position;
            }
        }
    }
}
