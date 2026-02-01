using System;
using UnityEngine;

public class GameEventManager : Singleton<GameEventManager>
{
    [SerializeField] public GameState _gameState;
    // Events
    public static event Action<Level> OnLevelCompleted;
    public static event Action OnGameOver;
    public static event Action OnGameWon;
    public static event Action OnGameStarted;

    // Add more events here

    //* Networking Events

    /// <summary>
    /// Request Session creation to the NetManager
    /// </summary>
    public static event Action OnSessionRequested;
    /// <summary>
    /// Request Join session to the NetManager
    /// </summary>
    public static event Action OnJoinRequested;
    /// <summary>
    /// Request Exit session to the NetManager
    /// </summary>
    public static event Action OnExitRequested;
    public static event Action OnJoinRequestFailed;

    ///

    protected override void Awake()
    {
        base.Awake();
        if(!_gameState)
            throw new NullReferenceException("GameState ScriptableObject is missing, add it to the GameEventsManager on the editor");
    }
    //protected override void OnDestroy() => base.OnDestroy();

    private void OnEnable()
    {
        //NetworkManager.OnSessionCreated += OnSessionCreatedHandler;
    }

    private void OnDisable()
    {
        // Remember to clear static events to prevent memory leaks and ghost listners
        OnLevelCompleted = null;
        OnGameOver = null;
        OnGameWon = null;
        OnGameStarted = null;
        OnJoinRequestFailed = null;

        //NetworkManager.OnSessionCreated -= OnSessionCreatedHandler;
    }

    #region Public triggers

    public static void TriggerOnLevelCompleted(Level level) => OnLevelCompleted?.Invoke(level);
    public static void TriggerOnGameOver() => OnGameOver?.Invoke();
    public static void TriggerOnGameWon() => OnGameWon?.Invoke();
    public static void TriggerOnGameStarted() => OnGameStarted?.Invoke();

    public static void TriggerOnJoinRequestFailed() => OnJoinRequestFailed?.Invoke();
    // Add more events triggers here

    //* Networking triggers

    public static void TiggerOnSessionRequested()
    {
        ConnectionManager.Instance.HostGame();
        Debug.Log("Session creation requested");
    }

    public static void TiggerOnJoinRequested()
    {
        string code = Instance._gameState.SessionId;
        Debug.Log($"Session join requested to {code}");

        if(code.Length > 0)
        {
            ConnectionManager.Instance.JoinGame(code);
        }
    }
    public static void TriggerOnExitRequested()
    {
        ConnectionManager.Instance.ExitGame();
        OnExitRequested?.Invoke();
        Debug.Log("Session exist requested");
    }
#endregion

#region 
    private void OnSessionCreatedHandler()
    {
        OnGameStarted?.Invoke();
        _gameState.InitPlayer();
    }

#endregion


}