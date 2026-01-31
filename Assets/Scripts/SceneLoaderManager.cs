using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Level {One, Two, Three}
//
public class SceneLoaderManager : Singleton<SceneLoaderManager>
{
    // Scenes

    // This call the implementation of the base class to instantiate the Singleton
    protected override  void Awake() => base.Awake();
    protected override void OnDestroy() => base.OnDestroy();

    private void OnEnable()
    {
        GameEventManager.OnGameStarted += OnGameStartedHandler;
    }
    //TODO Disconned

    private void OnDisable()
    {
        
    }

    #region API

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

#endregion

#region Private members

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void OnGameStartedHandler()
    {
        LoadScene("Level_1");
    }

#endregion
}
