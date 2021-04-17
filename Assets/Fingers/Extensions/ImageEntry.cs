using UnityEngine;

namespace DigitalRubyShared.DrawMan
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Image Entry", menuName = "Fingers Lib/Image Entry")]
    public class ImageEntry : ScriptableObject
    {
        [SerializeField]
        private Sprite sprite;

        [SerializeField]
        public string Key;

        [Range(0.0f, 0.5f)]
        [SerializeField]
        public float ScorePadding;

        [TextArea(1, 8)]
        [SerializeField]
        public string Images;

        public string Description;

        public Sprite Sprite => sprite;
    }
}
