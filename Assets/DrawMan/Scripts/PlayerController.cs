using UnityEngine;
using DrawMan.Components;
using DrawMan.Core.ActionSystem;

namespace DrawMan.Core
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Components references")]
        [SerializeField] private Rigidbody2D m_rigidbody;
        [SerializeField] private GroundCheck m_groundCheck;
        [SerializeField] private CeilingCheck m_ceilingCheck;

        [Header("Parameters")]
        [SerializeField] [Min(0.0f)] private float m_maxSpeed;
        [SerializeField] [Min(0.0f)] private float m_jumpImpulseForce;
        [SerializeField] [Min(0.0f)] private float m_minSpeed;
        [Space]
        [Min(0.0f)]
        [Tooltip("Gravity acceleration while falling (m/s)")]
        [SerializeField] private float m_gravityAccel;

        [Min(0.0f)]
        [Tooltip("Gravity acceleration while jumping (m/s)")]
        [SerializeField] private float m_jumpGravityAccel;

        [Min(0.0f)]
        [Tooltip("Maximum velocity the player can fall at (m/s)")]
        [SerializeField] private float m_maxFallingSpeed;

        [Header("Character Actions (Input)")]
        [SerializeField] private MovementAction m_movementAction;
        [SerializeField] private JumpAction m_jumpAction;

        private Vector2 m_verticalVelocity = Vector2.zero;
        private Vector2 velocity = Vector2.zero;

        private PlayerInput m_playerInput;
        private Transform m_transform;

        private const float RIGHT_ANGLE = 90.0f;

        private bool m_wasGrounded;
        private bool m_isFalling;
        private bool m_facingRight = true;

        private void Awake()
        {
            m_playerInput = new PlayerInput();
            m_transform = transform;
        }

        private void OnEnable()
        {
            m_playerInput.Enable();
        }

        private void OnDisable()
        {
            m_playerInput.Disable();
        }

        private void FixedUpdate()
        {
            // Init
            m_groundCheck.CheckCollision();

            Vector2 up = m_transform.up;
            Vector2 right = Vector2.Perpendicular(m_groundCheck.Down);
            Vector2 dir = right * m_movementAction.MovementDirection.x;
            Vector2 dirNorm = Vector3.Normalize(dir);

            Vector2 minVelocity = dirNorm * m_minSpeed;

            velocity = dir * m_maxSpeed;

            m_isFalling = IsFalling(-up, m_verticalVelocity);
            float gravityAccel = m_isFalling ? m_gravityAccel : m_jumpGravityAccel;

            // Apply gravity
            ApplyGravity(gravityAccel);

            // Clamp velocity
            if (velocity != Vector2.zero && velocity.sqrMagnitude < minVelocity.sqrMagnitude)
            {
                velocity = minVelocity;
            }

            // Jump
            if (m_jumpAction.Jump && m_groundCheck.Grounded)
            {
                // OnJump event
                m_verticalVelocity = up * m_jumpImpulseForce;
                m_groundCheck.Clear();
            }

            if (m_isFalling)
                m_verticalVelocity = Vector2.ClampMagnitude(m_verticalVelocity, m_maxFallingSpeed);

            // Apply velocity
            m_rigidbody.velocity = velocity + m_verticalVelocity;

            m_wasGrounded = m_groundCheck.Grounded;
            m_ceilingCheck.Clear();
        }

        private void ApplyGravity(float gravityAccel)
        {
            if (m_wasGrounded && !m_groundCheck.Grounded)
            {
                m_verticalVelocity = m_groundCheck.Down * m_gravityAccel;
            }
            else if (!m_wasGrounded && m_groundCheck.Grounded)
            {
                // OnLand event
            }
            else
            {
                m_verticalVelocity += (m_groundCheck.Down * gravityAccel) * Time.fixedDeltaTime;
            }

            if (m_groundCheck.Grounded && m_isFalling)
            {
                m_verticalVelocity = m_groundCheck.Down * m_gravityAccel;
            }
            else if (m_ceilingCheck.Contact)
            {
                m_verticalVelocity = Vector2.zero;
                m_ceilingCheck.Clear();
            }
        }

        private void Flip(float movementValue)
        {
            // flipping condition
            bool flip = (movementValue > 0.0f && !m_facingRight ||
                movementValue < 0.0f && m_facingRight);
            
            if (flip)
            {
                // invert facing status
                m_facingRight = !m_facingRight;

                // onFlip event
                //Debug.Log("Implement onFlip event!");
            }
        }

        private bool IsFalling(Vector2 down, Vector2 direction) => Vector2.Angle(down, direction) <= RIGHT_ANGLE;

//#if UNITY_EDITOR
//        private void OnGUI()
//        {
//            GUI.TextArea(new Rect(0, 0, 255, 255),string.Format("X: {0}\nY: {1}\nVelocity: {2}\nVertical: {3}\nGrounded: {4}",
//                m_movementAction.MovementDirection.x,
//                m_movementAction.MovementDirection.y,
//                velocity,
//                m_verticalVelocity,
//                m_groundCheck.Grounded));
//        }
//#endif
    }
}
