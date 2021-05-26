using UnityEngine;
using DrawMan.Components;

namespace DrawMan.AI
{
    public abstract class EnemyMovement : ScriptableObject
    {
        public abstract void Move(Vector2 direction, EnemyBehaviour behaviour);
    }
}
