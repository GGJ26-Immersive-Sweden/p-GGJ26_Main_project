using System;
using UnityEngine;

/// <summary>
/// Responsabilities
/// (1) Shows and hides GUI components based on Game Events
/// </summary>
public class GuiManager:Singleton<GuiManager>
{
    [SerializeField] private GameObject _gameOverGui;
    [SerializeField] private GameObject _gameWonGui;

    protected override void Awake() => base.Awake();
    
    private void OnDestroy()
    {
        // Explicitly destroying this guy because the base class will try to persist it
        Destroy(Instance);
    }
    
    private void OnEnable() => SubscribeToGameEvents();
    private void OnDisable() => UnsubscribeToGameEvents();

    private void SubscribeToGameEvents()
    {
        GameEventManager.OnGameOver += OnGameOverHandler;
        GameEventManager.OnGameWon += OnGameWonHandler;
    }


    private void UnsubscribeToGameEvents()
    {
        GameEventManager.OnGameOver -= OnGameOverHandler;
        GameEventManager.OnGameWon -= OnGameWonHandler;
    }
    private void OnGameWonHandler()
    {
        if(_gameOverGui != null)
            _gameWonGui.SetActive(true);

        if(_gameOverGui != null)
            _gameOverGui.SetActive(false);
    }

    private void OnGameOverHandler()
    {
        if(_gameOverGui != null)
            _gameOverGui.SetActive(true);

        if(_gameOverGui != null)
            _gameWonGui.SetActive(false);
    }
}