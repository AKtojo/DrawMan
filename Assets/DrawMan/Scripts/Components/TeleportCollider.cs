using UnityEngine;

namespace DrawMan.Components
{
    public class TeleportCollider : MonoBehaviour
    {
        [SerializeField] private Transform m_destinationTransform;
        [SerializeField] private bool m_applyRotation;
        [SerializeField] private bool m_applyScale;

        public void Teleport(Transform target)
        {
            target.position = m_destinationTransform.position;
            target.rotation = m_applyRotation ? m_destinationTransform.rotation : target.rotation;
            target.localScale = m_applyScale ? m_destinationTransform.localScale : target.localScale;
        }
    }
}
