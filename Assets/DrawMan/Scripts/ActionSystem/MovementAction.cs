using UnityEngine;
using UnityEngine.InputSystem;

namespace DrawMan.Core.ActionSystem
{
    [CreateAssetMenu(fileName = "New Movement Action", menuName = "Characters/Actions/Movement Action")]
    public class MovementAction : ScriptableObject
    {
        private Vector2 m_movementDirection;
        [SerializeField] private float m_movementDeadzone = 0.25f;

        public Vector2 MovementDirection => m_movementDirection;
        public float MovementDeadzone => m_movementDeadzone;

        public void OnMove(InputAction.CallbackContext ctx)
        {
            m_movementDirection = ctx.ReadValue<Vector2>();
        }
    }
}
