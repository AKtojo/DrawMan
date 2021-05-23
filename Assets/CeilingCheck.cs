using UnityEngine;

namespace DrawMan.Components
{
    public class CeilingCheck : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 90.0f)] private float m_maxSlopeAngle;
        private int m_contactsCount = 0;
        private bool m_contact = false;

        Vector2 m_up = Vector2.down;

        public bool Contact => m_contactsCount > 0;
        public Vector2 Up => m_up;

        public void Clear()
        {
            m_contactsCount = 0;
            m_contact = false;
            m_up = transform.up;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            EvaluateCollision(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Clear();
        }

        void EvaluateCollision(Collision2D collision)
        {
            Vector2 down = -transform.up;
            m_up = Vector2.zero;

            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector2 normal = collision.GetContact(i).normal;
                m_up -= normal;
                m_contactsCount++; //|= Vector2.Angle(down, normal) <= m_maxSlopeAngle;
            }

            if (m_contactsCount > 1)
                m_up.Normalize();
            else
                m_up = -down;

            m_contact = Vector2.Angle(down, -m_up) <= m_maxSlopeAngle;
        }
    }
}
