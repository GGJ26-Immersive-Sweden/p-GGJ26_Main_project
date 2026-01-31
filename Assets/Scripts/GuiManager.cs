using System;
using UnityEngine;

/// <summary>
/// Responsabilities
/// (1) Shows and hides GUI components based on Game Events
/// </summary>
public class GuiManager:Singleton<GuiManager>
{
    [SerializeField] private GameObject _gameOverGui;

    protected override void Awake() => base.Awake();
    //protected override void OnDestroy() => base.OnDestroy();

    private void OnEnable() => SubscribeToGameEvents();
    private void OnDisable() => UnsubscribeToGameEvents();

    private void SubscribeToGameEvents()
    {
        GameEventManager.OnGameOver += OnGameOverHandler;
    }
    private void UnsubscribeToGameEvents()
    {
        GameEventManager.OnGameOver -= OnGameOverHandler;
    }

    private void OnGameOverHandler()
    {
        if(_gameOverGui != null)
            _gameOverGui.SetActive(true);
    }
}