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

                angle = Vector2.Angle(up, normal);
                distance = Vector2.Distance(origin, point);

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

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 down = new Vector3(m_down.x, m_down.y, m_transform.position.z);
            Gizmos.DrawLine(m_transform.position, m_transform.position + down);

            Gizmos.color = Color.cyan;
            Vector2 perp2D = Vector2.Perpendicular(down);
            Vector3 perp = new Vector3(perp2D.x, perp2D.y, m_transform.position.z);
            Gizmos.DrawLine(m_transform.position, m_transform.position + perp);
        }

        float angle, distance;
        private void OnGUI()
        {
            GUI.TextArea(new Rect(256, 0, 256, 24), "Angle: " + angle);
            GUI.TextArea(new Rect(256, 32, 256, 24), "Distance: " + distance);
        }
#endif
    }
}
