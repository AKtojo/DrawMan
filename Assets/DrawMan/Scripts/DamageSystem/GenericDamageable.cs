using UnityEngine;
using UnityEngine.Events;

namespace DrawMan.DamageSystem
{
    public class GenericDamageable : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;

        [SerializeField] private UnityEvent onDie;

        public void GetDamage(float damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0.0f)
            {
                currentHealth = 0.0f;
                onDie.Invoke();
            }
        }

        public void ZeroHealth()
        {
            currentHealth = 0.0f;
            onDie.Invoke();
        }
    }
}
