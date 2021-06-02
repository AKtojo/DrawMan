using UnityEngine;
using Cinemachine;
using DrawMan.Core;

namespace DrawMan.Components
{
    public class BlendBetween : MonoBehaviour
    {
        [SerializeField] private Transform m_target;
        [SerializeField] private Transform m_start;
        [SerializeField] private Transform m_end;
        [Space]
        [SerializeField] private CinemachineMixingCamera m_cinemachineMixingCamera;

        void Update()
        {
            float t = Mathf.Clamp01(Utilities.Inverp(m_start.position, m_end.position, m_target.position));
            m_cinemachineMixingCamera.m_Weight0 = 1 - t;
            m_cinemachineMixingCamera.m_Weight1 = t;  
        }
    }
}
