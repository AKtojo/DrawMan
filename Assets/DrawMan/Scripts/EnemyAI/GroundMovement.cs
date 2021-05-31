using UnityEngine;
using DrawMan.Components;

namespace DrawMan.AI
{
    [CreateAssetMenu(fileName = "New Ground Movement", menuName = "FSM/Enemies/Actions/GroundMovement")]
    public class GroundMovement : EnemyMovement
    {
        public override void Move(Vector2 direction, EnemyBehaviour behaviour)
        {
            Vector2 velocity = Vector2.zero;
            Vector2 gravity = behaviour.Gravity;

            Vector2 rbVelocity = behaviour.Rigidbody.velocity;

            velocity.x = Mathf.MoveTowards(
                rbVelocity.x,
                direction.x * behaviour.Stats.MoveSpeed,
                behaviour.Stats.ChangeDirectionSpeed * Time.fixedDeltaTime);
            
            if (behaviour.Grounded)
            {
                velocity.y = gravity.y;
            }
            else
            {
                velocity.y += gravity.y * behaviour.Stats.GravityAccel * Time.fixedDeltaTime;
            }

            behaviour.Rigidbody.velocity = velocity;
        }
    }
}
