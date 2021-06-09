using UnityEngine;
using UnityEngine.UI;
using DrawMan.Core.Variables;

namespace DrawMan.UI
{
    public class BarValueCheck : MonoBehaviour
    {
        [SerializeField] private FloatVariable m_value;
        [SerializeField] private Image m_fillImage;

        private void Start()
        {
            UpdateBar();
        }

        private void OnEnable()
        {
            m_value.Subscribe(UpdateBar);
        }

        private void OnDisable()
        {
            m_value.Unsubscribe(UpdateBar);
        }

        private void UpdateBar()
        {
            Vector2 fill = m_fillImage.rectTransform.anchorMax;
            fill.x = m_value.Ratio01;
            m_fillImage.rectTransform.anchorMax = fill;
        }

#if UNITY_EDITOR
        [SerializeField] private bool DEBUG;
        private void OnGUI()
        {
            if (!DEBUG) return;
            if (GUILayout.Button("Damage!"))
            {
                m_value.Value -= 10;
            }
            else if (GUILayout.Button("Heal!"))
            {
                m_value.Value += 10;
            }
        }
#endif
    }
}
