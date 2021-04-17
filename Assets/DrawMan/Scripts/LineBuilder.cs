using System.Collections.Generic;
using UnityEngine;
using DrawMan.Components;

namespace DrawMan.Core
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(LineRenderer), typeof(PolygonCollider2D), typeof(TimedEvent))]
    public class LineBuilder : MonoBehaviour
    {
        [Header("Collisions")]
        [SerializeField] private LayerMask m_collisionAllowed;
        [SerializeField] private LayerMask m_collisionForbidden;

        [Header("Line settings")]
#if UNITY_EDITOR
        [SerializeField] [Min(0.0f)] private float m_lineWidth;

        private void OnValidate()
        {
            if (m_lineRenderer == null)
                m_lineRenderer = GetComponent<LineRenderer>();

            if (m_collider == null)
                m_collider = GetComponent<PolygonCollider2D>();

            m_lineRenderer.startWidth = m_lineRenderer.endWidth = m_lineWidth;

            m_halfWidth = m_lineWidth * 0.5f;
        }
#endif

        [SerializeField] [Min(0.0f)] private float m_minLineStep;
        [SerializeField] private float m_lineDepthZ;
        [SerializeField] private float m_lineClipZ;

        [SerializeField] private float m_lineDuration;

        [SerializeField] [HideInInspector] private float m_halfWidth;

        [Header("References")]
        [SerializeField] private TimedEvent m_disableEvent;
        [SerializeField] private LineRenderer m_lineRenderer;
        [SerializeField] private PolygonCollider2D m_collider;

        private List<Vector3> m_points = new List<Vector3>();
        private List<Vector2> m_colliderPoints = new List<Vector2>();

        private bool m_isValid = true;

        private HashSet<Transform> m_allowedTransforms = new HashSet<Transform>();

        public float MinLineStep => m_minLineStep;
        public bool IsValid => m_isValid;
        public float LineDepthZ => m_lineDepthZ;
        public float LineClipZ => m_lineClipZ;

        public void SetLineDepth(float depth)
        {
            m_lineDepthZ = depth;
        }

        public void AddPointToLine(Vector3 point)
        {
            if (m_points.Contains(point)) return;

            bool skip =
                m_points.Count > 1 &&
                Vector2.Distance(point, m_points[m_points.Count - 1]) < m_minLineStep;

            if (skip) return;

            point.z = m_lineDepthZ;

            m_points.Add(point);
            m_lineRenderer.positionCount++;
            m_lineRenderer.SetPosition(m_lineRenderer.positionCount - 1, point);

            AddPointToCollider(m_points.Count - 1);
        }

        public void AddPointToCollider(int index)
        {
            Vector2 dir = m_points[index] - m_points[index + 1 < m_points.Count ? index + 1 : (index - 1 >= 0 ? index - 1 : index)];
            float alpha = Mathf.Atan2(dir.x, -dir.y);

            Vector2 point = m_points[index];

            float x = m_halfWidth * Mathf.Cos(alpha);
            float y = m_halfWidth * Mathf.Sin(alpha);

            m_colliderPoints.Add(new Vector2(point.x + x, point.y + y));
            m_colliderPoints.Insert(0, new Vector2(point.x - x, point.y - y));

            m_collider.points = m_colliderPoints.ToArray();
            //m_collider.SetPath(0, m_colliderPoints.ToArray());
        }

        public void EnablePhysics()
        {
            m_collider.enabled = true;
        }

        public void DisablePhysics()
        {
            m_collider.enabled = false;
        }

        public void StartTimer()
        {
            m_disableEvent.StartTimer(m_lineDuration);
        }

        public void DisableLine()
        {
            gameObject.SetActive(false);
            m_disableEvent.StopTimer();
            m_allowedTransforms.Clear();
            DisablePhysics();
            ResetLine();
        }

        public bool PointIsFarEnough(Vector2 point, float sqrDistance)
        {
            bool far = true;
            foreach (var tr in m_allowedTransforms)
            {
                far &= (tr.position - (Vector3)point).sqrMagnitude >= sqrDistance;
            }
            return far;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            bool allowed = Utilities.MaskContainsLayer(m_collisionAllowed, collision.gameObject.layer);
            bool forbidden = Utilities.MaskContainsLayer(m_collisionForbidden, collision.gameObject.layer);
            if (allowed)
            {
                m_allowedTransforms.Add(collision.transform);
            }
            m_isValid = allowed && (!forbidden);
        }

        public void ResetLine()
        {
            m_lineRenderer.positionCount = 0;
            m_collider.pathCount = 0;
            m_points.Clear();
            m_colliderPoints.Clear();
        }
    }
}
