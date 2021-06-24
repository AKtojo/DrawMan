using UnityEngine;
using DrawMan.Components;

namespace DrawMan.AI
{
    public abstract class EnemyMovement : ScriptableObject
    {
        public abstract void Move(ref float currentSpeed, Vector2 direction, EnemyBehaviour behaviour);
    }
}
