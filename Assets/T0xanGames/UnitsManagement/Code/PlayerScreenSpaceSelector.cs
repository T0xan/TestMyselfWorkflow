using System;
using System.Collections.Generic;
using System.Linq;
using T0xanGames.Utils;
using UnityEngine;
using UnityEngine.Profiling;

namespace T0xanGames.UnitsManagement
{
    public class PlayerScreenSpaceSelector : PlayerComponent, ISelector
    {
        [Header("References:")]
        [SerializeField]
        private Camera m_Camera;

        private bool _isSelecting;

        private Rect _selectionScreenRect;
        private Plane[] _selectionPlanes;

        private List<ISelectable> _selected;

        public event Action<ISelectable> OnSelect;
        public event Action<ISelectable> OnDeselect;

        public bool IsSelecting => _isSelecting;
        public Rect SelectionRect => _selectionScreenRect;

        public void Initialize()
        {
            _selectionPlanes = new Plane[6];
            _selected = new List<ISelectable>();
        }

        public void StartSelect()
        {
            var rect = new Rect(Base.Input.MousePosition.x, Base.Input.MousePosition.y, 0.0f, 0.0f);

            _selectionScreenRect = rect;

            _isSelecting = true;
        }

        public void EndSelect()
        {
            _isSelecting = false;
        }

        public void UpdateSelecting(ISelectable[] total)
        {
            Vector2 delta = Base.Input.MousePosition - _selectionScreenRect.position;
            _selectionScreenRect.size = delta;

            UpdateProjectionPlanes();
            Project(total);
        }

        private void UpdateProjectionPlanes()
        {
            if (_selectionScreenRect.size == Vector2.zero)
                return;

            T0Geometry.ProjectFrustrumFromCamera(m_Camera, _selectionScreenRect, ref _selectionPlanes);
        }

        private void Project(ISelectable[] total)
        {
            foreach (var selectable in total)
            {
                bool isSelected = GeometryUtility.TestPlanesAABB(_selectionPlanes, selectable.GetWorldSpaceBounds());
                bool previousSelected = _selected.Contains(selectable);

                if (isSelected && !previousSelected)
                {
                    _selected.Add(selectable);
                    selectable.Select();

                    OnSelect?.Invoke(selectable);
                }
                else if (!isSelected && previousSelected)
                {
                    _selected.Remove(selectable);
                    selectable.Deselect();

                    OnDeselect?.Invoke(selectable);
                }
            }
        }

        public Vector3? SelectWorldPoint()
        {
            Ray ray = m_Camera.ScreenPointToRay(Base.Input.MousePosition);
            bool casted = Physics.Raycast(ray, out RaycastHit hit);

            return casted ? hit.point : null;
        }

        public ISelectable[] GetSelected()
        {
            return _selected.ToArray();
        }

#if UNITY_EDITOR
        [Header("Debug:")]
        public bool DrawSelectionProject;
        public bool DrawNormals;
        public bool DrawPolygons;
        private void OnDrawGizmos()
        {
            if (DrawSelectionProject && Application.isPlaying && _selectionPlanes != null)
            {
                Gizmos.color = Color.blue;
                T0Gizmos.DrawFrustrumFromPlanes(_selectionPlanes, DrawNormals, DrawPolygons);
            }
        }
#endif
    }
}
