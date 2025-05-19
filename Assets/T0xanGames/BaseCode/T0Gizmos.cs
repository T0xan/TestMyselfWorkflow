using UnityEngine;

namespace T0xanGames.Utils
{
    public static class T0Gizmos
    {
        private const float NORMAL_MULTIPLAY = 0.1f;

        public static Color normalsColor = Color.white;
        public static Color polygonsColor = Color.cyan;

        public static void DrawFrustrumFromPlanes(Plane[] planes, bool drawPlaneNormals = false, bool drawPolygons = false)
        {
            Vector3[] points = T0Geometry.GetCornersOfFrustumPlanes(planes);

            Gizmos.DrawLine(points[0], points[1]);
            Gizmos.DrawLine(points[1], points[2]);
            Gizmos.DrawLine(points[2], points[3]);
            Gizmos.DrawLine(points[3], points[0]);

            Gizmos.DrawLine(points[4], points[5]);
            Gizmos.DrawLine(points[5], points[6]);
            Gizmos.DrawLine(points[6], points[7]);
            Gizmos.DrawLine(points[7], points[4]);

            Gizmos.DrawLine(points[0], points[4]);
            Gizmos.DrawLine(points[1], points[5]);
            Gizmos.DrawLine(points[2], points[6]);
            Gizmos.DrawLine(points[3], points[7]);

            if (drawPlaneNormals)
            {
                float normalsFactor = Vector3.Distance(points[0], points[4]) * NORMAL_MULTIPLAY;

                DrawPolygonNormal(planes[4].normal * normalsFactor, points[0], points[1], points[3], points[2], drawPolygons);
                DrawPolygonNormal(planes[5].normal * normalsFactor, points[4], points[5], points[7], points[6], drawPolygons);

                DrawPolygonNormal(planes[0].normal * normalsFactor, points[0], points[4], points[3], points[7], drawPolygons);
                DrawPolygonNormal(planes[1].normal * normalsFactor, points[1], points[5], points[2], points[6], drawPolygons);
                DrawPolygonNormal(planes[2].normal * normalsFactor, points[3], points[2], points[7], points[6], drawPolygons);
                DrawPolygonNormal(planes[3].normal * normalsFactor, points[0], points[1], points[4], points[5], drawPolygons);
            }
        }
        public static void DrawPolygonNormal(Vector3 dir, Vector3 vertexUpLeft, Vector3 vertexUpRight, Vector3 vertexDownLeft, Vector3 vertexDownRight, bool drawPolygon = false)
        {
            var previousColor = Gizmos.color;
            Gizmos.color = normalsColor;

            Vector3 middleUp = Vector3.Lerp(vertexUpLeft, vertexUpRight, 0.5f);
            Vector3 middleDown = Vector3.Lerp(vertexDownLeft, vertexDownRight, 0.5f);
            Vector3 point = Vector3.Lerp(middleUp, middleDown, 0.5f);

            Gizmos.DrawLine(point, point + dir);

            if (drawPolygon)
            {
                Gizmos.color = polygonsColor;

                Gizmos.DrawLine(vertexUpLeft, vertexDownRight);
                Gizmos.DrawLine(vertexUpRight, vertexDownLeft);
            }

            Gizmos.color = previousColor;
        }
    }
}