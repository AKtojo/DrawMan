using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DrawMan.Events
{
    public class MultipleTrigger : MonoBehaviour
    {
        [SerializeField] private bool active = false;
        [SerializeField] private uint m_triggersCount;
        [SerializeField] private UnityEvent m_onAllTriggersOn;
        [SerializeField] private UnityEvent m_onAllTriggersOff;

        [SerializeField] [HideInInspector] private bool[] m_triggers;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (m_triggers == null || m_triggers.Length != m_triggersCount)
            {
                m_triggers = new bool[m_triggersCount];
            }
        }
#endif

        private void Awake()
        {
            if (active) m_onAllTriggersOn.Invoke();
            else m_onAllTriggersOff.Invoke();
        }

        public void TriggerOn(int index)
        {
            m_triggers[index] = true;

            bool activate = true;
            for (int i = 0; i < m_triggersCount; i++)
            {
                activate &= m_triggers[i];
            }

            if (activate)
            {
                active = true;
                m_onAllTriggersOn.Invoke();
            }
        }

        public void TriggerOff(int index)
        {
            m_triggers[index] = false;

            if (active)
            {
                active = false;
                m_onAllTriggersOff.Invoke();
            }
        }
    }
}
