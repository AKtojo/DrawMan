using UnityEngine;
using DrawMan.Core.ActionSystem;
using DrawMan.Core.DataContainers;

namespace DrawMan.Core
{
    public class PlayerController : MonoBehaviour
    {
        private Vector2 m_gravityDirection = Vector2.down;
        private Vector2 m_verticalVelocity = Vector2.zero;

        [Header("Components references")]
        [SerializeField] private DirectionData m_relativeDirection;
        //[SerializeField] private CapsuleCollider2D m_collider;
        [SerializeField] private Rigidbody2D m_rigidbody;
        [SerializeField] private Transform m_groundCheckTransform;
        [SerializeField] private Transform m_ceilingCheckTransform;

        [Header("Parameters")]
        [SerializeField] LocomotionSettings m_locomotionSettings;
        [Space]
        [Min(0.0f)]
        [Tooltip("Instance's current gravity acceleration multiplier while falling")]
        [SerializeField] private float m_gravityMultiplier;

        [Min(0.0f)]
        [Tooltip("Instance's current gravity acceleration multiplier while jumping")]
        [SerializeField] private float m_jumpGravityMultiplier;

        [Min(0.0f)]
        [Tooltip("Maximum m/s this instance can travel")]
        [SerializeField] private float m_maximumFallingSpeed;

        [Header("\tGround check")]
        [SerializeField] LayerMask m_groundCheckLayers;
        [SerializeField] LayerMask m_ceilingCheckLayers;

        [Space]
        [Tooltip("Radius of the circle that checks for ground")]
        [SerializeField] private float m_maxWalkableSlope;

        [Tooltip("Radius of the circle that checks for ground")]
        [SerializeField] private float m_circleRadius;

        [Tooltip("Minimum distance to check for collision from instance")]
        [SerializeField] private float m_checkDistance;

        [Tooltip("Maximum depth to check for ground.\nMust be > 0")]
        [SerializeField] private float m_maxDepthCheck;

        [Tooltip("Minimum depth to check for ground.\nMust be < 0")]
        [SerializeField] private float m_minDepthCheck;

        [Min(1)]
        [Tooltip("Maximum number of contact points to check per frame")]
        [SerializeField] private int m_maxContacts;

        [Header("Character Actions")]
        [SerializeField] private MovementAction m_movementAction;
        [SerializeField] private JumpAction m_jumpAction;

        private PlayerInput m_playerInput;
        private Transform m_transform;

        private const float ACUTE_45_ANGLE = 45.0f;
        private const float RIGHT_ANGLE = 90.0f;
        private const float STRAIGHT_ANGLE = 180.0f;
        private const float OBTUSE_270_ANGLE = 270.0f;
        private const float ROUND_ANGLE = 360.0f;

        private float m_jumpInertia;

        private bool m_grounded;
        private bool m_wasGrounded;
        private bool m_isFalling;
        private bool m_ceilingBumped;
        private bool m_facingRight = true;

        private RaycastHit2D m_groundHit;
        private RaycastHit2D m_ceilingHit;

        private ContactPoint2D[] m_contactPoints;

        private void Awake()
        {
            m_playerInput = new PlayerInput();
            m_transform = transform;

            m_contactPoints = new ContactPoint2D[m_maxContacts];
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
            //VectorBasedMovement();
            m_isFalling = IsFalling();

            CheckGround();

            CheckCeiling();

            ScaleGravity();

            CheckLanding();

            PerformJump();

            //Vector2 movementDirection = canMove || !m_grounded ?
            //    (Vector2)Vector3.Cross(Vector3.back, m_groundHit.normal) :
            //    Vector2.zero;

            Vector2 movementDirection = m_relativeDirection.Right;

            float movementVal = m_movementAction.MovementDirection.x;

            Vector2 horizontalVelocity = GetHorizontalVelocity(movementDirection, movementVal);

            Flip(movementVal);

            // apply movement;
            m_rigidbody.velocity = m_verticalVelocity + horizontalVelocity;
        }

        private void CheckGround()
        {
            m_wasGrounded = m_grounded;

            float distance = m_checkDistance + 0.2f;

            m_groundHit = Physics2D.CircleCast(m_groundCheckTransform.position, m_circleRadius,
                m_gravityDirection, distance, m_groundCheckLayers, m_minDepthCheck, m_maxDepthCheck);

            if (m_groundHit.collider == null)
            {
                m_grounded = false;
                return;
            }

            float dist = Vector2.Distance(m_groundCheckTransform.position, m_groundHit.point);

            float angle = Vector2.Angle(m_groundHit.normal, -m_gravityDirection);

            m_grounded = dist < m_checkDistance && angle <= m_maxWalkableSlope;

            if (m_grounded && !m_wasGrounded)
            {
                m_verticalVelocity = m_groundHit.normal * m_locomotionSettings.GravityForce * -m_gravityMultiplier;
            }
            else if (m_wasGrounded && !m_grounded && m_isFalling)
            {
                m_verticalVelocity = Physics2D.gravity * m_gravityMultiplier;
            }
        }

        private void CheckCeiling()
        {
            m_ceilingBumped = false;
            float angle = Mathf.Atan2(m_gravityDirection.y, m_gravityDirection.x);

            m_ceilingHit =
                Physics2D.BoxCast(m_ceilingCheckTransform.position, Vector2.one,
                angle, -m_gravityDirection, m_checkDistance, m_ceilingCheckLayers,
                m_minDepthCheck, m_maxDepthCheck);

            m_ceilingBumped =
                m_ceilingHit.collider != null &&
                m_ceilingHit.distance < m_checkDistance &&
                !m_isFalling &&
                Vector2.Angle(m_gravityDirection, m_ceilingHit.normal) < RIGHT_ANGLE;
        }

        private void ScaleGravity()
        {
            float gravityScale = m_isFalling ? m_gravityMultiplier : m_jumpGravityMultiplier;

            m_verticalVelocity = m_grounded ?
                Vector2.zero :
                (m_verticalVelocity + (Physics2D.gravity * Time.fixedDeltaTime * gravityScale));

            // set the maximum falling speed
            m_verticalVelocity = m_ceilingBumped ?
                m_gravityDirection :
                Vector2.ClampMagnitude(m_verticalVelocity, m_maximumFallingSpeed);
        }

        private void CheckLanding()
        {
            // just landed and is falling
            if (!m_wasGrounded && m_grounded && m_isFalling)
            {
                m_jumpInertia = 0.0f;
                // onLand event
                //Debug.Log("Implement onLand event!");
            }
        }

        private void PerformJump()
        {
            // check and perform jump
            if (m_jumpAction.Jump && m_grounded)
            {
                m_verticalVelocity = m_gravityDirection * -m_locomotionSettings.JumpForce;
                m_jumpInertia = m_movementAction.MovementDirection.x * m_locomotionSettings.MaxAiredSpeed;
                m_grounded = false;

                // onJump event
                //Debug.Log("Implement onJump event!");
            }
        }

        private Vector2 GetHorizontalVelocity(Vector2 direction, float movementValue)
        {
            // change movement speed based on grounded state
            //float speed = m_grounded ?
            //    m_locomotionSettings.MaxMovementSpeed :
            //    m_locomotionSettings.MaxAiredSpeed;

            //return direction * movementValue * speed;

            Vector2 hDir = direction * movementValue;

            return m_grounded ?
                hDir * m_locomotionSettings.MaxMovementSpeed :
                hDir * m_locomotionSettings.MaxAiredSpeed;
        }

        // it works, needs tuning and corrections
        private Vector2 GetAiredMovement(Vector2 direction, float movementValue)
        {
            float inertia = m_jumpInertia;

            float value = m_jumpInertia - m_locomotionSettings.JumpInertiaFalloff * m_locomotionSettings.MaxAiredSpeed * Time.fixedDeltaTime;
            m_jumpInertia = Mathf.Clamp(value, 0.0f, m_jumpInertia);

            return (direction * movementValue) + (direction * inertia);
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

        public void SetGravityDirection(Vector3 direction)
        {
            m_gravityDirection = m_relativeDirection.Down;
            Physics2D.gravity = m_gravityDirection * m_locomotionSettings.GravityForce;
        }

        public void SetGravityDirection(float x, float y, float z)
        {
            m_gravityDirection = m_relativeDirection.Down;
            Physics2D.gravity = m_gravityDirection * m_locomotionSettings.GravityForce;
        }

        //private bool IsFalling => Vector3.Angle(m_gravityDirection, m_characterController.velocity) <= RIGHT_ANGLE;
        private bool IsFalling() => !m_grounded && Vector2.Angle(m_gravityDirection, m_rigidbody.velocity) <= RIGHT_ANGLE;
    }
}
