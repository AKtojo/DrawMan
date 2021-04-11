using UnityEngine;

namespace DrawMan.Core.DataContainers
{
    [CreateAssetMenu(fileName = "New Locomotion Settings", menuName = "Characters/Locomotion Settings")]
    public class LocomotionSettings : ScriptableObject
    {
        [Min(0.0f)]
        [SerializeField]
        [Tooltip("Standard gravity acceleration value expressed in meters per second.\nDefault value: 9.807 m/s²")]
        private static float m_defaultGravityForce = 9.807f; // TO DO: make this static and write custom editor to set static variable

        [Min(0.0f)]
        [SerializeField]
        [Tooltip("Maximum movement speed while on the ground.")]
        private float m_maxMovementSpeed;

        [Min(0.0f)]
        [SerializeField]
        [Tooltip("Maximum movement speed while in air.")]
        private float m_maxAiredSpeed;

        [Min(0.01f)]
        [SerializeField]
        [Tooltip("Inertia falloff while moving in air without receiving input")]
        private float m_jumpInertiaFalloff;

        [Min(0.0f)]
        [SerializeField]
        [Tooltip("Minimum movement speed.\nCan not be higher than max speeds.")]
        private float m_minSpeed;

        [Min(0.0f)]
        [SerializeField]
        [Tooltip("Instantaneous acceleration value of the impulse on jumping.")]
        float m_jumpImpulseForce;

        public float GravityForce => m_defaultGravityForce; // TO DO: make this static
        public float MaxMovementSpeed => m_maxMovementSpeed;
        public float MaxAiredSpeed => m_maxAiredSpeed;
        public float JumpInertiaFalloff => m_jumpInertiaFalloff;
        public float MinSpeed => m_minSpeed;
        public float JumpForce => m_jumpImpulseForce;
    }
}
