using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//
public class SceneLoaderManager : Singleton<SceneLoaderManager>
{
    // This call the implementation of the base class to instantiate the Singleton
    protected override  void Awake() => base.Awake();
    protected override void OnDestroy() => base.OnDestroy();

    #region API

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    #endregion

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
