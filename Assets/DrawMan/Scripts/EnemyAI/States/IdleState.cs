using UnityEngine;
using DrawMan.Components;

namespace DrawMan.AI
{
    [CreateAssetMenu(fileName = "New IdleState", menuName = "FSM/Enemies/IdleState")]
    public class IdleState : State<EnemyBehaviour>
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (id == 0)
            {
                id = (int)EnemyBStates.Idle;
            }
        }
#endif

        public override void Enter(FiniteStateMachine<EnemyBehaviour> fsm, EnemyBehaviour container)
        {
            container.SetDirection(Vector2.zero);

            // Change animation
        }

        public override void Execute(FiniteStateMachine<EnemyBehaviour> fsm, EnemyBehaviour container)
        {
            Vector3 origin = container.transform.position;
            Collider2D[] collider = new Collider2D[1];
            RaycastHit2D[] hit = new RaycastHit2D[1];

            if (Physics2D.OverlapCircleNonAlloc(
                origin,
                container.Stats.ChaseRange,
                collider, container.Player) > 0)
            {
                //LayerMask mask = ~player.value;
                if (Physics2D.RaycastNonAlloc(
                    origin,
                    collider[0].transform.position - origin,
                    hit, container.Stats.ChaseRange,
                    container.Occluder) == 0)
                {
                    // Something entered the ChaseRange
                    fsm.ChangeState((int)EnemyBStates.Chase);
                }
            }
        }

        public override void Exit(FiniteStateMachine<EnemyBehaviour> fsm, EnemyBehaviour container)
        {
            // Do nothing
        }

        public override void Initialize(EnemyBehaviour container)
        {
            // Initialize
        }
    }
}
