using UnityEngine;
using DrawMan.Components;

namespace DrawMan.AI
{
    [CreateAssetMenu(fileName = "New ChaseState", menuName = "FSM/Enemies/ChaseState")]
    public class ChaseState : State<EnemyBehaviour>
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (id == 0)
            {
                id = (int)EnemyBStates.Chase;
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

                    //    if (distance <= container.Stats.AttackRange)
                    //    {
                    //        fsm.ChangeState((int)EnemyBStates.Attack);
                    //    }
                    if (distance <= container.Stats.FleeRange)
                    {
                        fsm.ChangeState((int)EnemyBStates.Flee);
                    }

                    // Move towards target
                    //container.transform.position +=
                    //    Vector3.right * container.Stats.MoveSpeed * Time.deltaTime * Mathf.Sign(dir.x);
                    container.SetDirection(dir);
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

        public override void Initialize(EnemyBehaviour container)
        {
            // Initialize
        }
    }
}
