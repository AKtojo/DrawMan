using UnityEngine;
using DrawMan.AI;

namespace DrawMan.Components
{
    enum EnemyBStates { NONE, Idle, Chase, Flee, Attack, Death, GetHit }

    [System.Serializable]
    public struct EnemyStats
    {
        [Header("Sensors")]
        [SerializeField] public LayerMask Player;
        [SerializeField] public LayerMask Occluder;
        [SerializeField] [Min(0)] public float AttackRange;
        [SerializeField] [Min(0)] public float ChaseRange;
        [SerializeField] [Min(0)] public float FleeRange;
        
        [Header("Motion")]
        [SerializeField] [Min(0)] public float MoveSpeed;
        [SerializeField] [Min(0)] public float DamagePerHit;
        [SerializeField] [Min(0)] public float AttackSpeed;

        [Space]
        [SerializeField] [Min(0)] public float GravityAccel;
        [SerializeField] [Min(0)] public float MaxFallingSpeed;
        [SerializeField] [Range(0.0f, 50.0f)] public float ChangeDirectionSpeed;

        [SerializeField] [HideInInspector] private float maxRange;
        public float MaxRange => maxRange;

        public void Init()
        {
            maxRange = Mathf.Max(AttackRange, ChaseRange, FleeRange);
        }
    }

    public class EnemyBehaviour : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private GroundCheck groundCheck;

        [Header("Parameters")]
        [SerializeField] private EnemyStats stats;
        [SerializeField] private State<EnemyBehaviour>[] inStates;

        [Header("Actions")]
        [SerializeField] private EnemyMovement movement;

        private Vector2 direction;
        private FiniteStateMachine<EnemyBehaviour> machine;

        public Rigidbody2D Rigidbody => rb;

        public EnemyStats Stats => stats;
        public Vector2 Direction => direction;
        public LayerMask Player => stats.Player;
        public LayerMask Occluder => stats.Occluder;

        public Vector2 Gravity => transform.up * -stats.GravityAccel;

        public bool Grounded => groundCheck.Grounded;

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
        }

        public void Awake()
        {
            stats.Init();
            machine =
                new FiniteStateMachine<EnemyBehaviour>
                (this, inStates, (int)EnemyBStates.Idle);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stats.AttackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, stats.ChaseRange);
            Gizmos.color = new Color(1.0f, 0.5f, 0.0f);
            Gizmos.DrawWireSphere(transform.position, stats.FleeRange);
        }
#endif

        private void Update()
        {
            machine.CurrentState.Execute(machine, this);
        }

        private void FixedUpdate()
        {
            groundCheck.CheckCollision();

            // Move here
            movement.Move(Direction, this);
        }

        private void OnGUI()
        {
            GUILayout.TextArea("Grounded: " + Grounded);
        }

        /*public void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                machine.ChangeState((int)EnemyBStates.Idle);
            if (Input.GetKeyDown(KeyCode.B))
                machine.ChangeState((int)EnemyBStates.Battle);

            if (Input.GetKeyDown(KeyCode.Space))
                machine.CurrentState.Execute();
        }*/
    }
}
