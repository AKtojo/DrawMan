using UnityEngine;

namespace DrawMan.Core.DataContainers
{
    [CreateAssetMenu(fileName = "New Direction DataContainers", menuName = "Utils/Direction DataContainers")]
    public class DirectionData : ScriptableObject
    {
        private Quaternion m_rotation;
        [SerializeField] private Vector2 m_eulerRotation;

        public Quaternion Rotation => m_rotation;
        public Vector2 EulerRotation => m_eulerRotation;
        public Vector2 Up => m_rotation * Vector2.up;
        public Vector2 Down => m_rotation * Vector2.down;
        public Vector2 Right => m_rotation * Vector2.right;
        public Vector2 Left => m_rotation * Vector2.left;

        private const float RADIUS = 360.0f;

        public void Rotate(Vector3 amount) => Rotate(amount.x, amount.y, amount.z);

        public void Rotate(float x, float y, float z)
        {
            m_eulerRotation.x = (m_eulerRotation.x + x) % RADIUS;
            m_eulerRotation.y = (m_eulerRotation.y + y) % RADIUS;
            //m_rotationDirection.z = (m_rotationDirection.z + z) % RADIUS;
            m_rotation = Quaternion.Euler(m_eulerRotation);
        }

        public void SetRotation(Vector2 rotation)
        {
            m_eulerRotation = rotation;
            m_rotation = Quaternion.Euler(rotation);
        }

        public void SetRotation(float x, float y)
            //, float z)
        {
            m_eulerRotation.x = x;
            m_eulerRotation.y = y;
            //m_rotationDirection.z = z;
            m_rotation = Quaternion.Euler(x, y, 0.0f);
        }

    #if UNITY_EDITOR
        private void OnValidate()
        {
            m_eulerRotation.x = (m_eulerRotation.x + RADIUS) % RADIUS;
            m_eulerRotation.y = (m_eulerRotation.y + RADIUS) % RADIUS;
            //m_rotationDirection.z = (m_rotationDirection.z + RADIUS) % RADIUS;
            m_rotation = Quaternion.Euler(m_eulerRotation);
        }
    #endif
    }
}
