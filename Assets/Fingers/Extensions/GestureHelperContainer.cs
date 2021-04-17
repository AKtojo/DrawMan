using UnityEngine;

namespace DigitalRubyShared.DrawMan
{
    [CreateAssetMenu(fileName = "New Image Container", menuName = "Fingers Lib/Image Container")]
    public class GestureHelperContainer : ScriptableObject
    {
        [SerializeField] [HideInInspector] public FingersImageGestureHelperComponentScript Helper;
    }
}
