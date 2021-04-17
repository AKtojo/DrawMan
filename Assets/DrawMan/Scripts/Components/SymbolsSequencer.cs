using UnityEngine;
using DigitalRubyShared.DrawMan;
using UnityEngine.Events;

namespace DrawMan.Components
{
    public class SymbolsSequencer : MonoBehaviour
    {
        [SerializeField] private GestureHelperContainer m_helperContainer;
        [SerializeField] private ImageEntry[] m_sequence;

        [SerializeField] private UnityEvent m_onDie;

        private int m_currentIndex = 0;

        private void OnEnable()
        {
            m_currentIndex = 0;
        }

        public void OnSymbolFound()
        {
            var match = m_helperContainer.Helper.CheckForImageMatch();

            if (match != null &&
                m_sequence[m_currentIndex].GetHashCode() ==
                match.GetHashCode())
            {
                m_currentIndex++;
                m_currentIndex %= m_sequence.Length;

                if (m_currentIndex == 0) m_onDie.Invoke();
            }
        }
    }
}
