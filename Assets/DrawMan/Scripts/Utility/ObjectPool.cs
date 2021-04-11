using System.Collections.Generic;
using UnityEngine;

namespace DrawMan.Core.Utility
{
    [CreateAssetMenu(fileName = "New Object Pool", menuName = "Utils/Object Pool")]
    public class ObjectPool : ScriptableObject
    {
        [SerializeField] private GameObject m_objectPrefab;

        private List<GameObject> m_objectsPool = new List<GameObject>();

        private int m_currentIndex = 0;

        public void Clear()
        {
            m_currentIndex = 0;
            m_objectsPool.Clear();
        }

        public GameObject GetObject()
        {
            GameObject currentObject;

            if (m_objectsPool.Count == 0)
            {
                currentObject = Instantiate(m_objectPrefab);
                m_objectsPool.Add(currentObject);

                return currentObject;
            }

            int end = m_currentIndex % m_objectsPool.Count;

            do
            {
                currentObject = m_objectsPool[m_currentIndex];
                m_currentIndex = (m_currentIndex + 1) % m_objectsPool.Count;

                if (!currentObject.activeSelf)
                {
                    return currentObject;
                }
            } while (m_currentIndex != end);

            currentObject = Instantiate(m_objectPrefab);
            m_objectsPool.Add(currentObject);

            return currentObject;
        }
    }
}
