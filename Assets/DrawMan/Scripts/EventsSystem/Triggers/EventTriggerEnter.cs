using UnityEngine;
using UnityEngine.Events;

namespace DrawMan.Events
{
    [RequireComponent(typeof(Collider2D))]
    public class EventTrigger : MonoBehaviour
    {
        [Header("Options")]
        [SerializeField] protected LayerMask m_LayerToCheck;
        [SerializeField] protected GameObject m_PrefabToCheck;

#if UNITY_EDITOR

        private Collider2D m_collider;

        private void OnValidate()
        {
            if (m_collider == null)
            {
                m_collider = GetComponent<Collider2D>();
            }

            m_collider.isTrigger = true;
        }
#endif

        protected static bool CheckCollision(Collider2D collision, LayerMask layerMask, GameObject obj)
        {
            int layer = 1 << collision.gameObject.layer;
            int layerToCheck = layerMask.value;
            bool sameLayer = ((layerToCheck & layer) == layer);
            bool sameObject = obj == collision.gameObject;

            bool collided = layerToCheck == 0 && obj == null; // everything
            collided |= sameLayer && sameObject; // layer & prefab
            collided |= sameLayer && obj == null; // just layer
            collided |= layerToCheck == 0 && sameObject; // just prefab

            return collided;
        }
    }

    public class EventTriggerEnter : EventTrigger
    {
        [Space]
        [SerializeField] private UnityEvent m_OnEnterEvent;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            bool execute = CheckCollision(collision, m_LayerToCheck, m_PrefabToCheck);

            if (execute)
            {
                m_OnEnterEvent.Invoke();
            }
        }
    }
}
