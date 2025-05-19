using UnityEngine;

namespace T0xanGames.UnitsManagement
{
    public class PlayerInput : MonoBehaviour
    {
        public bool LeftMouse { get; private set; }
        public bool LeftMouseDown { get; private set; }
        public bool LeftMouseUp { get; private set; }

        public bool SpawnFollowPoint { get; private set; }

        public Vector2 MousePosition { get; private set; }

        private void Update()
        {
            DesktopInput();
        }

        private void DesktopInput()
        {
            LeftMouse = Input.GetKey(KeyCode.Mouse0);
            LeftMouseDown = Input.GetKeyDown(KeyCode.Mouse0);
            LeftMouseUp = Input.GetKeyUp(KeyCode.Mouse0);

            SpawnFollowPoint = Input.GetKeyDown(KeyCode.Mouse1);

            MousePosition = Input.mousePosition;
        }
    }
}
