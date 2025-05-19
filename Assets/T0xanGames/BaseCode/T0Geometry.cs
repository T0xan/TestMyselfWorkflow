using UnityEngine;

namespace T0xanGames.Utils
{
    public static class T0Geometry
    {
        public static void CreatePrimitivesFromPlanes(Plane[] planes)
        {
            for (int i = 0; i < 6; ++i)
            {
                GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
                p.name = "Plane " + i.ToString();
                p.transform.position = -planes[i].normal * planes[i].distance;
                p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
            }
        }

        public static Vector3[] GetCornersOfFrustumPlanes(Plane[] frustumPlanes)
        {
            // [0] = Left, [1] = Right, [2] = Down, [3] = Up, [4] = Near, [5] = Far

            Plane left = frustumPlanes[0];
            Plane right = frustumPlanes[1];
            Plane bottom = frustumPlanes[2];
            Plane top = frustumPlanes[3];
            Plane near = frustumPlanes[4];
            Plane far = frustumPlanes[5];

            Vector3[] corners = new Vector3[8];

            corners[0] = Intersect3Planes(near, top, left) * -1;    // near-top-left
            corners[1] = Intersect3Planes(near, top, right) * -1;   // near-top-right
            corners[2] = Intersect3Planes(near, bottom, right) * -1;// near-bottom-right
            corners[3] = Intersect3Planes(near, bottom, left) * -1; // near-bottom-left

            corners[4] = Intersect3Planes(far, top, left) * -1;     // far-top-left
            corners[5] = Intersect3Planes(far, top, right) * -1;    // far-top-right
            corners[6] = Intersect3Planes(far, bottom, right) * -1; // far-bottom-right
            corners[7] = Intersect3Planes(far, bottom, left) * -1;  // far-bottom-left

            return corners;
        }
        private static Vector3 Intersect3Planes(Plane p1, Plane p2, Plane p3)
        {
            Vector3 n1 = p1.normal;
            Vector3 n2 = p2.normal;
            Vector3 n3 = p3.normal;

            Vector3 cross23 = Vector3.Cross(n2, n3);
            float det = Vector3.Dot(n1, cross23);

            Vector3 point = (p1.distance * cross23 +
                             p2.distance * Vector3.Cross(n3, n1) +
                             p3.distance * Vector3.Cross(n1, n2)) / det;

            return point;
        }

        public static void ProjectFrustrumFromCamera(Camera cam, Rect screenRect, ref Plane[] planes)
        {
            Rect normalizedRect = GetNormalizedRect(screenRect);
            Vector2[] screenCorners = GetOrderedCorners(normalizedRect);

            Vector3[] nearCorners = new Vector3[4];
            Vector3[] farCorners = new Vector3[4];
            for (int i = 0; i < 4; i++)
            {
                Ray ray = cam.ScreenPointToRay(screenCorners[i]);
                nearCorners[i] = ray.origin + ray.direction * cam.nearClipPlane;
                farCorners[i] = ray.origin + ray.direction * cam.farClipPlane;
            }

            Plane nearPlane = new Plane(cam.transform.forward, cam.transform.position + cam.transform.forward * cam.nearClipPlane);
            Plane farPlane = new Plane(-cam.transform.forward, cam.transform.position + cam.transform.forward * cam.farClipPlane);
            
            planes[0] = new Plane(nearCorners[0], farCorners[1], farCorners[0]);    // Left
            planes[1] = new Plane(farCorners[2], nearCorners[3], farCorners[3]);    // Right
            planes[2] = new Plane(farCorners[3], nearCorners[0], farCorners[0]);    // Bottom
            planes[3] = new Plane(nearCorners[1], farCorners[2], farCorners[1]);    // Top
            planes[4] = nearPlane;                                                  // Near
            planes[5] = farPlane;                                                   // Far
        }
        private static Rect GetNormalizedRect(Rect rect)
        {
            float x = rect.width < 0 ? rect.x + rect.width : rect.x;
            float y = rect.height < 0 ? rect.y + rect.height : rect.y;
            float width = Mathf.Abs(rect.width);
            float height = Mathf.Abs(rect.height);
            return new Rect(x, y, width, height);
        }
        private static Vector2[] GetOrderedCorners(Rect rect)
        {
            Vector2[] corners = new Vector2[4];
            float xMin = rect.x;
            float xMax = rect.x + rect.width;
            float yMin = rect.y;
            float yMax = rect.y + rect.height;

            corners[0] = new Vector2(xMin, yMin); // Left-bottom
            corners[1] = new Vector2(xMin, yMax); // Left-top
            corners[2] = new Vector2(xMax, yMax); // Right-top
            corners[3] = new Vector2(xMax, yMin); // Right-bottom

            return corners;
        }

        public static void GenerateMesh(Matrix4x4 projectionMatrix, ref Mesh mesh)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(projectionMatrix);

            var vertices = GetCornersOfFrustumPlanes(planes);

            var triangles = new int[12 * 3];
            DrawPolygon(ref triangles, 0, 0, 1, 3, 2);
            DrawPolygon(ref triangles, 6, 7, 6, 4, 5);

            DrawPolygon(ref triangles, 12, 4, 0, 7, 3);
            DrawPolygon(ref triangles, 18, 0, 4, 1, 5);
            DrawPolygon(ref triangles, 24, 1, 5, 2, 6);
            DrawPolygon(ref triangles, 30, 6, 7, 2, 3);

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }
        private static void DrawPolygon(ref int[] triangles, int offset, int topLeft, int topRight, int bottomLeft, int bottomRight)
        {
            triangles[0 + offset] = bottomRight;
            triangles[1 + offset] = topRight;
            triangles[2 + offset] = topLeft;

            triangles[3 + offset] = bottomLeft;
            triangles[4 + offset] = bottomRight;
            triangles[5 + offset] = topLeft;
        }
    }
}