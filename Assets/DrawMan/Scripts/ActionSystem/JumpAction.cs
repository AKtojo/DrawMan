using UnityEngine;
using UnityEngine.InputSystem;

namespace DrawMan.Core.ActionSystem
{
    [CreateAssetMenu(fileName = "New Jump Action", menuName = "Characters/Actions/Jump Action")]
    public class JumpAction : ScriptableObject
    {
        private bool m_jump;

        public bool Jump
        {
            get
            {
                bool jump = m_jump;
                m_jump = false;
                return jump;
            }
        }

        public void OnJump(InputAction.CallbackContext ctx)
        {
            m_jump = ctx.started ? ctx.ReadValueAsButton() : m_jump;
        }
    
        public void Reset()
        {
            m_jump = false;
        }
    }
}
