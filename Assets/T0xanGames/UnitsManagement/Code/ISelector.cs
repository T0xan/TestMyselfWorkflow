using UnityEngine;

namespace T0xanGames.UnitsManagement
{
    public interface ISelector
    {
        public void Initialize();

        public void StartSelect();
        public void UpdateSelecting(ISelectable[] total);
        public void EndSelect();

        public Vector3? SelectWorldPoint();

        public ISelectable[] GetSelected();
    }
}
