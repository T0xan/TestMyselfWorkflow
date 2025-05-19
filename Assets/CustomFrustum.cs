using T0xanGames.Utils;
using UnityEngine;

public class CustomFrustum : MonoBehaviour
{
    public Rect screenRect;
    public Camera cam;

    private Plane[] customPlanes;

    private void Awake()
    {
        customPlanes = new Plane[6];
    }

    void Update()
    {
        T0Geometry.ProjectFrustrumFromCamera(cam, screenRect, ref customPlanes);
    }

    private void OnDrawGizmos()
    {
        if (customPlanes != null)
        {
            Gizmos.color = Color.blue;
            T0Gizmos.DrawFrustrumFromPlanes(customPlanes);
        }
    }
}