using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace DrawMan
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

        }
    }
}
