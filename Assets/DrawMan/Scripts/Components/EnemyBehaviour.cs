using UnityEngine;
using DrawMan.AI;

namespace DrawMan.Components
{
    enum EnemyBStates { NONE, Idle, Chase, Flee, Attack, Death, GetHit }

    [System.Serializable]
    public struct EnemyStats
    {
        [SerializeField] [Min(0)] public float AttackRange;
        [SerializeField] [Min(0)] public float ChaseRange;
        [SerializeField] [Min(0)] public float FleeRange;
        [SerializeField] [Min(0)] public float MoveSpeed;
        [SerializeField] [Min(0)] public float DamagePerHit;
        [SerializeField] [Min(0)] public float AttackSpeed;

        [SerializeField] [HideInInspector] private float maxRange;
        public float MaxRange => maxRange;

        public void Init()
        {
            maxRange = Mathf.Max(AttackRange, ChaseRange, FleeRange);
        }
    }

    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private EnemyStats stats;
        [SerializeField] private State<EnemyBehaviour>[] inStates;

        public EnemyStats Stats => stats;

        private FiniteStateMachine<EnemyBehaviour> machine;

        public void Awake()
        {
            stats.Init();
            machine = new FiniteStateMachine<EnemyBehaviour>(this, inStates, (int)EnemyBStates.Idle);
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
