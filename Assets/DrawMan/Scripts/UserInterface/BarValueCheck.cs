using UnityEngine;
using UnityEngine.UI;
using DrawMan.Core.Variables;

namespace DrawMan.UI
{
    public class BarValueCheck : MonoBehaviour
    {
        [SerializeField] private Image currentHealthImage;

        [SerializeField] private FloatVariable health;

        private void OnEnable()
        {
            health.Subscribe(UpdateBar);
        }

        private void OnDisable()
        {
            health.Unsubscribe(UpdateBar);
        }

        private void UpdateBar()
        {
            Vector2 anchorMax = currentHealthImage.rectTransform.anchorMax;
            anchorMax.x = health.Ratio01;
            currentHealthImage.rectTransform.anchorMax = anchorMax;
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            if (GUILayout.Button("Damage!"))
            {
                health.Value -= 10;
            }
            else if (GUILayout.Button("Heal!"))
            {
                health.Value += 10;
            }
        }
#endif
    }
}
