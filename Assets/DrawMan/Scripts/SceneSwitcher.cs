using UnityEngine;
using UnityEngine.SceneManagement;

namespace DrawMan.Core
{
    public class SceneSwitcher : MonoBehaviour
    {
        public void StartLoadingScene(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }

        public void StartUnloadingScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }
    }
}
