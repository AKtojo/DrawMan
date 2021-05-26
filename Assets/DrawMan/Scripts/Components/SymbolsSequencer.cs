using UnityEngine;
using DigitalRubyShared.DrawMan;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

namespace DrawMan.Components
{
    public class SymbolsSequencer : MonoBehaviour
    {
        [SerializeField] private HorizontalLayoutGroup m_symbolsContainer;
        [SerializeField] private GestureHelperContainer m_helperContainer;
        [SerializeField] private ImageEntry[] m_sequence;

        [SerializeField] private UnityEvent m_onDie;

        private List<Image> m_symbols;

        private int m_currentIndex = 0;

        private void OnEnable()
        {
            m_currentIndex = 0;
        }

        private void Awake()
        {
            m_symbols = new List<Image>(m_sequence.Length);
            m_symbols.Add(m_symbolsContainer.GetComponentInChildren<Image>());
            m_symbols[0].sprite = m_sequence[0].Sprite;

            for (int i = 1; i < m_sequence.Length; i++)
            {
                m_symbols.Add(Instantiate(m_symbols[0].gameObject, m_symbolsContainer.transform).GetComponent<Image>());
                m_symbols[i].sprite = m_sequence[i].Sprite;
            }
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
