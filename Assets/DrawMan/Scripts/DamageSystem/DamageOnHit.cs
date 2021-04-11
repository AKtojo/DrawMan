using UnityEngine;

namespace DrawMan.DamageSystem
{
    public class DamageOnHit : MonoBehaviour
    {
        [SerializeField] private LayerMask damageable;
        [SerializeField] private bool kill;
        [SerializeField] private float damage;

        private void OnTriggerEnter2D(Collider2D other)
        {
            int layer = 1 << other.gameObject.layer;
            if ((damageable.value & layer) == layer)
            {
                var damageable = (IDamageable)other.gameObject.GetComponent(typeof(IDamageable));

                if (kill)
                {
                    damageable.ZeroHealth();
                }
                else
                {
                    damageable.GetDamage(damage);
                }
            }
        }

#if UNITY_EDITOR
        public LayerMask Damageable
        {
            get => damageable;
            set { damageable = value; }
        }
        public bool KillInstantly
        {
            get => kill;
            set { kill = value; }
        }
        public float Damage
        {
            get => damage;
            set { damage = value; }
        }
#endif
    }
}