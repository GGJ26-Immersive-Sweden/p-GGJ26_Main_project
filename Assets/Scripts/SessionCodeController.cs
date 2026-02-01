using System;
using TMPro;
using UnityEngine;

public class SessionCodeController : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private TMP_Text _sessionCodeText;

    private void OnEnable() => GameEventManager.OnGameStarted += OnGameStartedHandler;
    private void OnDisable() => GameEventManager.OnGameStarted -= OnGameStartedHandler;

    private void OnGameStartedHandler()
    {
        _sessionCodeText.text = $"{_gameState.SessionId}";
    }


}
