using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DrawMan.Core
{
    [System.Serializable]
    public struct SceneCluster
    {
        public string[] Scenes;
    }

    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField]
        public SceneCluster[] SceneClusters;
        private SceneCluster CurrentLoadedScenes;

        private string lastLoadedSceneIndex;

        private List<AsyncOperation> currentOperations;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            currentOperations = new List<AsyncOperation>();
        }

        //public void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.I))
        //    {
        //        //LoadScenes(SceneClusters[0]);                
        //        ChangeScenes(SceneClusters[0], SceneClusters[1]);
        //    }
        //    if (Input.GetKeyDown(KeyCode.O))
        //    {
        //        //UnloadScenes(SceneClusters[0]);
        //        ChangeScenes({ { "Scene", "Scene2" } }, SceneClusters[0]);
        //    }
        //}


        public List<AsyncOperation> LoadScenes(SceneCluster newcluster)
        {
            List<AsyncOperation> operations = new List<AsyncOperation>();
            Scene currScene;
            bool isSceneValid, isSceneAlreadyLoaded;
            foreach (var load in newcluster.Scenes)
            {
                currScene = SceneManager.GetSceneByName(load);
                isSceneValid = true;//currScene.IsValid();
                isSceneAlreadyLoaded = currScene.isLoaded;

                if (!isSceneValid || isSceneAlreadyLoaded) continue;

                var op = SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive);
                operations.Add(op);

                
            }
            return operations;
        }

        public List<AsyncOperation> UnloadScenes(SceneCluster oldcluster)
        {
            List<AsyncOperation> operations = new List<AsyncOperation>();
            Scene currScene;
            bool isSceneValid, isSceneNotLoaded;
            foreach (var unload in oldcluster.Scenes)
            {
                currScene = SceneManager.GetSceneByName(unload);
                isSceneValid = true;//currScene.IsValid();
                isSceneNotLoaded = !currScene.isLoaded;

                if (!isSceneValid || isSceneNotLoaded) continue;

                var op = SceneManager.UnloadSceneAsync(unload, UnloadSceneOptions.None);
                operations.Add(op);

                //Debug.Log("Unloaded");
            }
            return operations;
        }



        public void ChangeScenes(SceneCluster unloadCluster, SceneCluster newloadCluster)
        {
            currentOperations = UnloadScenes(unloadCluster);
            if (currentOperations.Count > 0) StartCoroutine(Loading());

            //currentOperations.Clear();
            

            currentOperations = LoadScenes(newloadCluster);
            if (currentOperations.Count > 0) StartCoroutine(Loading());

            //currentOperations.Clear();
            Debug.Log(currentOperations.Count);
        }

        IEnumerator Loading()
        {
            //float progress = 0;
            for (int i = 0; i < currentOperations.Count; i++)
            {
                while (!currentOperations[i].isDone)
                {
                    //progress += currentOperations[i].progress;
                    //Debug.Log(progress / currentOperations.Count);
                    yield return null;
                }
            }

            Debug.Log("Loading Finished!");
            //yield return null;
        }
    }
}
