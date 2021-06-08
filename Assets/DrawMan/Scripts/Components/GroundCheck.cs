using UnityEngine;

namespace DrawMan.Components
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 90.0f)] private float m_maxSlopeAngle;
        [SerializeField] private LayerMask m_groundMask;

        [SerializeField] [HideInInspector] private Transform m_transform;
        Vector2 m_down = Vector2.down;

        private int m_contacts = 0;
        [SerializeField] [HideInInspector] private float m_radius = 0;

        public bool Grounded => m_contacts > 0;
        public Vector2 Down => m_down;

        public void Clear()
        {
            m_contacts = 0;
            m_down = -transform.up;
        }

        private void OnValidate()
        {
            m_radius = GetComponent<CircleCollider2D>().radius;
            m_transform = transform;
        }

        public void CheckCollision()
        {
            float radius = m_radius + 0.015f;
            Vector2 origin = transform.position;
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int count = Physics2D.CircleCastNonAlloc(origin, radius, m_down, hits, radius * 2, m_groundMask, -10, 10);

            Vector2 up = transform.up;
            m_contacts = 0;

            m_down = Vector2.zero;
            for (int i = 0; i < count; i++)
            {
                Vector2 normal = hits[i].normal;
                Vector2 point = hits[i].point;

                float angle = Vector2.Angle(up, normal);
                float distance = Vector2.Distance(origin, point);

                if (angle <= m_maxSlopeAngle &&
                    distance <= radius)
                {
                    m_down -= normal;
                    m_contacts++;
                }
            }

            if (Grounded)
                m_down.Normalize();
            else
                m_down = -up;
        }
    }
}
