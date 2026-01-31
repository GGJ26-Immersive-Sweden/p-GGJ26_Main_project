using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public enum Level {One, Two, Three}
//
public class SceneLoaderManager : Singleton<SceneLoaderManager>
{
    // Scenes

    // This call the implementation of the base class to instantiate the Singleton
    protected override  void Awake() => base.Awake();
    //protected override void OnDestroy() => base.OnDestroy();

    private void OnEnable()
    {
        if(GameEventManager.Instance)
        {
            GameEventManager.OnGameStarted += OnGameStartedHandler;
            GameEventManager.OnExitRequested += OnExitRequestedHandler;   
        }
    }

    

    private void OnDisable()
    {
        if(GameEventManager.Instance)
        {
            GameEventManager.OnGameStarted -= OnGameStartedHandler;
            GameEventManager.OnExitRequested -= OnExitRequestedHandler;
        }
    }

    #region API

    //? UPDATE: Added the loading of the scene using Networking
    public void LoadScene(string sceneName)
    {
        // Check if networking is active
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            // Networked: only server/host can change scenes
            if (NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogWarning("Only the host can change scenes during a networked session.");
            }
        }
        else
        {
            // Not networked: use regular scene loading
            StartCoroutine(LoadSceneAsync(sceneName));
        }
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

    private void OnExitRequestedHandler()
    {
        LoadScene("Home");
    }

#endregion
}
