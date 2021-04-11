using UnityEngine;
using UnityEngine.Events;

namespace DrawMan.Components
{
    public class TimedEvent : MonoBehaviour
    {
        [SerializeField] private float m_time = 0.0f;
        [SerializeField] private UnityEvent m_onElapsed;

        private double m_last = 0.0f;

        public void SetTime(float time)
        {
            m_time = time;
        }

        public void StartTimer()
        {
            enabled = true;
            m_last = Time.timeAsDouble;
        }

        public void StartTimer(float time)
        {
            StartTimer();
            m_time = time;
        }

        public void StopTimer()
        {
            enabled = false;
        }

        void Update()
        {
            var elapsed = (m_last + m_time) <= Time.timeAsDouble;
            if (elapsed)
            {
                m_onElapsed.Invoke();
            }
        }
    }
}
