using UnityEngine;
using DrawMan.Core.ActionSystem;
using DrawMan.Core.EventsSystem;
using DigitalRubyShared.DrawMan;
using DigitalRubyShared;

namespace DrawMan.Core
{
    public class InkAttackController : MonoBehaviour
    {
        [Header("Ink Attack action")]
        [SerializeField] private InkAttackAction m_action;

        [Header("Shape found event")]
        [SerializeField] private GameEvent m_event;

        [Header("Gesture helper container")]
        [SerializeField] private GestureHelperContainer m_container;

        void LateUpdate()
        {
            if (m_action.ShapeEnd)
            {
                ImageGestureImage match = m_container.Helper.CheckForImageMatch();
                Debug.Log("Match: " + (match == null ? "null" : "found"));
                if (match != null)
                {
                    Debug.Log("Found image match: " + match.Name);
                    m_event.Raise();
                }
                m_container.Helper.Reset();
            }
        }

#if UNITY_EDITOR
        [SerializeField] private bool m_debug;

        private void OnGUI()
        {
            if (!m_debug) return;

            GUI.TextArea(new Rect(0, 0, 125, 96),
                "Shape start: " + (m_action.ShapeStart ? "O" : "X") + "\n" +
                "Shape end: " + (m_action.ShapeEnd ? "O" : "X") + "\n" +
                "Phase: " + m_action.Phase.ToString()
                );
        }
#endif
    }
}
