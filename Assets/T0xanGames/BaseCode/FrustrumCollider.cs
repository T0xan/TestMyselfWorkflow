using UnityEngine;

namespace T0xanGames.Utils
{
    [RequireComponent(typeof(MeshCollider))]
    public class FrustrumCollider : MonoBehaviour
    {
        [SerializeField]
        private MeshCollider m_MeshCollider;
        [SerializeField]
        private float m_FieldOfView = 60;
        [SerializeField]
        private float m_NearPlane = 0.5f;
        [SerializeField]
        private float m_FarPlane = 10.0f;
        [SerializeField]
        private float m_Aspect = 1.0f;

        private Mesh _mesh;

        private void Awake()
        {
            UpdateMesh();
        }

        private void OnValidate()
        {
            UpdateMesh();
        }

        private void UpdateMesh()
        {
            if (m_MeshCollider == null)
                return;

            if (_mesh == null)
            {
                _mesh = new Mesh();
            }

            ValidateValues();
            T0Geometry.GenerateMesh(GetProjectionMatrix(), ref _mesh);
            m_MeshCollider.sharedMesh = _mesh;
        }

        private void ValidateValues()
        {
            m_FieldOfView = Mathf.Clamp(m_FieldOfView, 1.0f, 179.0f);
            m_NearPlane = Mathf.Clamp(m_NearPlane, 0.01f, m_FarPlane - 0.01f);
            m_FarPlane = Mathf.Clamp(m_FarPlane, m_NearPlane + 0.01f, 10000.0f);
            m_Aspect = Mathf.Clamp(m_Aspect, 0.1f, 5.0f);
        }

        private Matrix4x4 GetProjectionMatrix()
        {
            return Matrix4x4.Perspective(m_FieldOfView, m_Aspect, m_NearPlane, m_FarPlane);
        }
    }
}