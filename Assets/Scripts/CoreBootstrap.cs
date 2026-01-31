using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This component loads the home Scene after all the singletons are initialized
/// </summary>
public class CoreBootstrap: MonoBehaviour
{
    [SerializeField] private string _firstScene = "Home";

    void Start()
    {
        SceneManager.LoadScene(_firstScene, LoadSceneMode.Additive);
    }
}