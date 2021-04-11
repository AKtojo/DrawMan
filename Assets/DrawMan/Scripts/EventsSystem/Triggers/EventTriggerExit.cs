using UnityEngine;
using UnityEngine.Events;

namespace DrawMan.Events
{
    public class EventTriggerExit : EventTrigger
    {
        [Space]
        [SerializeField] private UnityEvent m_OnExitEvent;

        private void OnTriggerExit2D(Collider2D collision)
        {
            bool execute = CheckCollision(collision, m_LayerToCheck, m_PrefabToCheck);

            if (execute)
            {
                m_OnExitEvent.Invoke();
            }
        }
    }
}
