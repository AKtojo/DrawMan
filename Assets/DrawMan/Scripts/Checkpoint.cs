using UnityEngine;
using UnityEngine.Events;

namespace DrawMan.Core
{
    [RequireComponent(typeof(Collider2D))]
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private LayerMask layerCheck;
        [SerializeField] private UnityEvent onActivate;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            int layer = 1 << collision.gameObject.layer;
            if ((layerCheck.value & layer) == layer)
            {
                var cpHolder = (CheckpointHolder)collision.gameObject.GetComponent(typeof(CheckpointHolder));
                cpHolder.SetCheckpoint(transform);
                onActivate?.Invoke();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            var center = transform.position;
            var lineEnd = center + (transform.right * 3f);
            Gizmos.DrawLine(center, lineEnd);
            Gizmos.DrawSphere(lineEnd, 0.2f);

            Gizmos.color = Color.green;

            lineEnd = center + (transform.up * 3f);
            Gizmos.DrawLine(center, lineEnd);
            Gizmos.DrawSphere(lineEnd, 0.2f);

            Gizmos.color = Color.white;
            Gizmos.DrawSphere(center, 0.3f);
        }
#endif
    }
}
