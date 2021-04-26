using UnityEngine;
using DigitalRubyShared.DrawMan;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DrawMan.Components
{
    public class SymbolsSequencer : MonoBehaviour
    {   [SerializeField] private GestureHelperContainer m_helperContainer;
        [SerializeField] private ImageEntry[] m_sequence;

        [SerializeField] private UnityEvent m_onDie;

        private int m_currentIndex = 0;

        private void OnEnable()
        {
            m_currentIndex = 0;
        }

        public void OnSymbolFound()
        {
            Debug.Log("Symbol found callback");

            var match = m_helperContainer.Helper.CheckForImageMatch();
            var currHash = match.Name.GetHashCode();
            var checkHash = m_sequence[m_currentIndex].Key.GetHashCode();
            
            if (match != null &&
                currHash == checkHash)
            {
                m_currentIndex++;
                Debug.Log("Current symbol: " + m_currentIndex);

                if (m_currentIndex >= m_sequence.Length)
                {
                    Debug.Log("Die!");
                    m_currentIndex = 0;
                    m_onDie.Invoke();
                }
            }
        }
    }
}
