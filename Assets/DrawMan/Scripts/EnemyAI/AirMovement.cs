using UnityEngine;
using DrawMan.Components;

namespace DrawMan.AI
{
    [CreateAssetMenu(fileName = "New Air Movement", menuName = "FSM/Enemies/Actions/AirMovement")]
    public class AirMovement : EnemyMovement
    {
        public override void Move(Vector2 direction, EnemyBehaviour behaviour)
        {
            Vector2 velocity = Vector2.zero;

            Vector2 forward = behaviour.transform.right;

            velocity = Vector2.MoveTowards(forward, direction, behaviour.Stats.ChangeDirectionSpeed);
            velocity *= behaviour.Stats.MoveSpeed;

            behaviour.Rigidbody.velocity = velocity;
        }
    }
}
