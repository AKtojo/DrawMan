using UnityEngine;
using DrawMan.Components;

namespace DrawMan.AI
{
    [CreateAssetMenu(fileName = "New Ground Movement", menuName = "FSM/Enemies/Actions/GroundMovement")]
    public class GroundMovement : EnemyMovement
    {
        public override void Move(ref float currentSpeed, Vector2 direction, EnemyBehaviour behaviour)
        {
            Vector2 gravity = behaviour.Gravity;
            float dt = Time.fixedDeltaTime;

            Vector2 right = Vector2.Perpendicular(behaviour.Down) * direction.x;
            Vector2 down = behaviour.Down;

            Vector2 rbVelocity = behaviour.Rigidbody.velocity;

            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                direction.x * behaviour.Stats.MoveSpeed,
                behaviour.Stats.ChangeDirectionSpeed * dt);

            if (!behaviour.Grounded)
            {
                down += gravity * behaviour.Stats.GravityAccel * dt;
            }

            behaviour.Rigidbody.velocity = (right * Mathf.Abs(currentSpeed)) + down;
        }
    }
}
