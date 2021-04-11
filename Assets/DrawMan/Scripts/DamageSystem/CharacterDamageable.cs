using UnityEngine;
using UnityEngine.Events;
using DrawMan.Core.Variables;

namespace DrawMan.DamageSystem
{
    public class CharacterDamageable : MonoBehaviour, IDamageable
    {
        [SerializeField] private FloatVariable healthVariable;

        [SerializeField] private UnityEvent onDie;

        public void GetDamage(float damage)
        {
            float health = healthVariable.Value;
            health -= damage;

            if (health <= 0.0f)
            {
                healthVariable.Value = 0.0f;
                onDie.Invoke();
            }
            else
            {
                healthVariable.Value = health;
            }
        }

        public void ZeroHealth()
        {
            healthVariable.Value = 0.0f;
            onDie.Invoke();
        }
    }
}
