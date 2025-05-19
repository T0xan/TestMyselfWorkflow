using UnityEngine;

namespace T0xanGames.Utils
{
    public static class T0Math
    {
        public static float EnsureNonZero(this float f)
        {
            return Mathf.Approximately(f, 0f) ? float.Epsilon * Mathf.Sign(f) : f;
        }
        public static float Snap(this float f, float t)
        {
            return Mathf.Abs(f) <= t ? Mathf.Sign(f) : f;
        }

        public static Vector2 SnapComponents(this Vector2 v, float t)
        {
            return new Vector2(v.x.Snap(t), v.y.Snap(t));
        }
    }
}
