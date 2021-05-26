using UnityEngine;
using DrawMan.Components;

namespace DrawMan.AI
{
    [CreateAssetMenu(fileName = "New FleeState", menuName = "FSM/Enemies/FleeState")]
    public class FleeState : State<EnemyBehaviour>
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (id == 0)
            {
                id = (int)EnemyBStates.Flee;
            }
        }
#endif
        public override void Enter(FiniteStateMachine<EnemyBehaviour> fsm, EnemyBehaviour container)
        {
            // Change animation
        }

        public override void Execute(FiniteStateMachine<EnemyBehaviour> fsm, EnemyBehaviour container)
        {
            Vector3 origin = container.transform.position;
            Collider2D[] collider = new Collider2D[1];
            RaycastHit2D[] hit = new RaycastHit2D[1];

            if (Physics2D.OverlapCircleNonAlloc(
                origin,
                container.Stats.MaxRange,
                collider, container.Player) > 0)
            {
                var dir = Vector3.Normalize(collider[0].transform.position - origin);

                if (Physics2D.RaycastNonAlloc(
                    origin, dir, hit,
                    container.Stats.MaxRange,
                    container.Occluder) == 0)
                {
                    float distance = Vector2.Distance(origin, collider[0].transform.position);

                    if (distance <= container.Stats.FleeRange)
                    {
                        // Move away from target
                        container.SetDirection(-dir);
                    }
                    else if (distance <= container.Stats.ChaseRange)
                    {
                        fsm.ChangeState((int)EnemyBStates.Chase);
                    }
                    else if (distance <= container.Stats.AttackRange)
                    {
                        //fsm.ChangeState((int)EnemyBStates.Attack);
                    }
                }
            }
            else
            {
                fsm.ChangeState((int)EnemyBStates.Idle);
            }
        }

        public override void Exit(FiniteStateMachine<EnemyBehaviour> fsm, EnemyBehaviour container)
        {
            // Do nothing
        }

        public override void Initialize(EnemyBehaviour owner)
        {
            // Initialize
        }
    }
}
