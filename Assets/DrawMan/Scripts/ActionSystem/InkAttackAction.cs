using UnityEngine;
using UnityEngine.InputSystem;
using DrawMan.Core.EventsSystem;

namespace DrawMan.Core.ActionSystem
{
    [CreateAssetMenu(fileName = "New Ink Attack Action", menuName = "Characters/Actions/Ink Attack Action")]
    public class InkAttackAction : ScriptableObject
    {
        private bool m_shapeStart;
        private bool m_shapeEnd;
        private InputActionPhase m_phase;

        public bool ShapeStart
        {
            get
            {
                var start = m_shapeStart;
                m_shapeStart = false;
                return start;
            }
        }
        public bool ShapeEnd
        {
            get
            {
                var end = m_shapeEnd;
                m_shapeEnd = false;
                return end;
            }
        }
        public InputActionPhase Phase => m_phase;

        public void OnDrawShape(InputAction.CallbackContext ctx)
        {
            m_phase = ctx.phase;
            m_shapeStart = ctx.started || ctx.performed;
            m_shapeEnd = ctx.canceled;
        }
    }
}
