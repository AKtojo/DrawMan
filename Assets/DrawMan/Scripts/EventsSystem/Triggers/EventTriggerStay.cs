using UnityEngine;
using UnityEngine.Events;

namespace DrawMan.Events
{
    public class EventTriggerStay : EventTrigger
    {
        [Space]
        [SerializeField] private UnityEvent m_OnStayEvent;

        private void OnTriggerStay2D(Collider2D collision)
        {
            bool execute = CheckCollision(collision, m_LayerToCheck, m_PrefabToCheck);

            if (execute)
            {
                m_OnStayEvent.Invoke();
            }
        }
    }
}
