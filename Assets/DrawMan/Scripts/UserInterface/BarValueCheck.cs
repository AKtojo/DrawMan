using UnityEngine;
using UnityEngine.UIElements;
using DrawMan.Core.Variables;

namespace DrawMan.UI
{
    public class BarValueCheck : MonoBehaviour
    {
        [SerializeField] private UIDocument m_uiDocument;
        [SerializeField] private string m_imageName;

        [SerializeField] private FloatVariable m_value;
        [SerializeField] private VisualElement m_fillImage;

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
            m_fillImage = m_uiDocument.rootVisualElement.Q<VisualElement>(m_imageName);
            var w = m_fillImage.style.width;
            w.value = new Length(m_value.Ratio100, LengthUnit.Percent);
            m_fillImage.style.width = w;
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
