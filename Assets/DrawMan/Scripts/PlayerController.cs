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
        [SerializeField] private Transform m_stickmanRig;
        [SerializeField] private Animator m_animator;

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

        // Animation parameters
        [SerializeField] [HideInInspector] int m_idleHash;
        [SerializeField] [HideInInspector] int m_runHash;
        [SerializeField] [HideInInspector] int m_jumpHash;
        [SerializeField] [HideInInspector] int m_attackHash;
        [SerializeField] [HideInInspector] int m_idleAttackHash;


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

            float x = m_movementAction.MovementDirection.x;
            float speed = x * m_maxSpeed;

            Vector2 up = m_transform.up;
            Vector2 right = Vector2.Perpendicular(m_groundCheck.Down);
            //Vector2 dir = right * x;
            //Vector2 dirNorm = Vector3.Normalize(dir);

            Vector2 minVelocity = right * x * m_minSpeed; // dirNorm * m_minSpeed;

            if (x != 0 && m_groundCheck.Grounded)
            {
                m_animator.Play(m_runHash);
            }
            else if (m_groundCheck.Grounded)
            {
                m_animator.Play(m_idleHash);
            }

            Flip(x);

            velocity = right * speed; // dir * m_maxSpeed;

            m_isFalling = IsFalling(-up, m_verticalVelocity);

            float gravityAccel = m_isFalling ? m_gravityAccel : m_jumpGravityAccel;

            // Apply gravity
            ApplyGravity(gravityAccel);

            // Clamp velocity
            if (velocity != Vector2.zero && Mathf.Abs(speed) < m_minSpeed)
            {
                velocity = minVelocity;
            }

            // Jump
            if (m_jumpAction.Jump && m_groundCheck.Grounded)
            {
                // OnJump event
                m_animator.Play(m_jumpHash);
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
                m_animator.Play(m_idleHash);
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

                Vector3 rot = m_stickmanRig.localRotation.eulerAngles;
                rot.y = (rot.y + 180.0f) % 360.0f;
                m_stickmanRig.localRotation = Quaternion.Euler(rot);

                // onFlip event
                //Debug.Log("Implement onFlip event!");
            }
        }

        private bool IsFalling(Vector2 down, Vector2 direction) => Vector2.Angle(down, direction) <= RIGHT_ANGLE;

#if UNITY_EDITOR
        [Header("Animation strings")]
        [SerializeField] string idleAnimation;
        [SerializeField] string runAnimation;
        [SerializeField] string jumpAnimation;
        [SerializeField] string attackAnimation;
        [SerializeField] string idleAttackAnimation;
        private void OnValidate()
        {
            m_idleHash = Animator.StringToHash(idleAnimation);
            m_runHash = Animator.StringToHash(runAnimation);
            m_jumpHash = Animator.StringToHash(jumpAnimation);
            m_attackHash = Animator.StringToHash(attackAnimation);
            m_idleAttackHash = Animator.StringToHash(idleAttackAnimation);
        }
#endif
    }
}
