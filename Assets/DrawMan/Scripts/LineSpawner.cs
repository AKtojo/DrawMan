using UnityEngine;
using DrawMan.Core.Utility;

namespace DrawMan.Core
{
    public class LineSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerInput m_input;
        [Header("Line data")]
        [SerializeField] private GameObject m_linePrefab;
        [SerializeField] private ObjectPool m_linesPool;

        private LineBuilder m_currentLine = null;

        public void CreateNewLine()
        {
            m_currentLine = (LineBuilder)m_linesPool.GetObject().GetComponent(typeof(LineBuilder));
        }
    }
}
