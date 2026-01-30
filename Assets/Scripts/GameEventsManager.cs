using System;
using UnityEngine;

public class GameEventManager : Singleton<GameEventManager>
{
    // Events

    public static event Action<Level> OnLevelCompleted;
    public static event Action OnGameOver;
    public static event Action OnGameWon;
    public static event Action OnGameStarted;

    // Add more events here

    ///

    protected override void Awake() => base.Awake();
    protected override void OnDestroy() => base.OnDestroy();

    private void OnDisable()
    {
        // Remember to clear static events to prevent memory leaks and ghost listners
        OnLevelCompleted = null;
        OnGameOver = null;
        OnGameWon = null;
        OnGameStarted = null;
    }

#region Public triggers

    public static void TriggerOnLevelCompleted(Level level) => OnLevelCompleted?.Invoke(level);
    public static void TriggerOnGameOver() => OnGameOver?.Invoke();
    public static void TriggerOnGameWon() => OnGameWon?.Invoke();
    public static void TriggerOnGameStarted() => OnGameStarted?.Invoke();
    // Add more events triggers here
    
    ///

#endregion

}