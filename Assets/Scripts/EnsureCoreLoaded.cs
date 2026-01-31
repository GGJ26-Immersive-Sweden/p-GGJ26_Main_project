using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is a utility that allows to run the game from Home
/// It removes itself
/// </summary>
public class EnsureCoreLoaded : MonoBehaviour
{
    void Awake()
    {
        #if UNITY_EDITOR
        if (SceneLoaderManager.Instance == null)
        {
            SceneManager.LoadScene("Core", LoadSceneMode.Additive);
        }
        #endif
        
        Destroy(gameObject);
    }
}