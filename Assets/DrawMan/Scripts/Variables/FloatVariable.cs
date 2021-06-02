using UnityEngine;
using UnityEngine.Events;

namespace DrawMan.Core.Variables
{
    [System.Serializable]
    public delegate void ValueChanged();

    [CreateAssetMenu(fileName = "New Float Variable", menuName = "Variables/Float")]
    [System.Serializable]
    public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private float m_value;
        [SerializeField] private bool m_clamped;
        [SerializeField] private float m_min;

#if UNITY_EDITOR
        public bool Clamped
        {
            get => m_clamped;
            set { m_clamped = value; }
        }
        public float Max
        {
            get => m_value;
            set { m_value = value; }
        }
        public float Min
        {
            get => m_min;
            set { m_min = value; }
        }

        private void OnValidate()
        {
            m_ratio01Value = 1.0f / m_value;
            m_ratio100Value = m_ratio01Value * 100.0f;
        }
#endif

        [SerializeField][HideInInspector] private float m_ratio100Value;
        [SerializeField] [HideInInspector] private float m_ratio01Value;

        [System.NonSerialized] private float m_runtimeValue;
        [SerializeField] private ValueChanged m_onValueChanged;

        public float Value
        {
            get => m_runtimeValue;
            set
            {
                m_runtimeValue = m_clamped ? Mathf.Clamp(value, m_min, m_value) : value;
                m_onValueChanged?.Invoke();
            }
        }

        #region DESIGNER_FUNCTIONS
        public void Add(float value)
        { Value += value; }

        public void Subtract(float value)
        { Value -= value; }

        public void Multiply(float value)
        { Value *= value; }

        public void Divide(float value)
        { Value /= value; }

        public void Refill()
        { Value = m_value; }

        public void SetToZero()
        { Value = 0.0f; }
        #endregion

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            m_runtimeValue = m_value;
        }

        public void Subscribe(ValueChanged ev)
        {
            m_onValueChanged += ev;
        }

        public void Unsubscribe(ValueChanged ev)
        {
            m_onValueChanged -= ev;
        }

        public float Ratio100 => m_runtimeValue * m_ratio100Value;
        public float Ratio01 => m_runtimeValue * m_ratio01Value;
    }
}
