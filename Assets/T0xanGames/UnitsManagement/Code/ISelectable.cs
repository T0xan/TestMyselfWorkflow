using UnityEngine;

namespace T0xanGames.UnitsManagement
{
    public interface ISelectable
    {
        public void Select();
        public void Deselect();

        public Bounds GetWorldSpaceBounds();
    }
}
