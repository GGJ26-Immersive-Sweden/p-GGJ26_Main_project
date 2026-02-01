using System.Collections;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using System;

public class ConnectionManager : Singleton<ConnectionManager> {

    [SerializeField] private GameState _gameState;
    // This call the implementation of the base class to instantiate the Singleton
    protected override void Awake()
    {
        base.Awake();
        if(!_gameState)
            throw new NullReferenceException("GameState ScriptableObject is missing, add it to the ConnectionManager on the editor");
    }

    // UI Input field to enter the relay join code
    //public TMP_InputField code;

    public async void HostGame() {
        Debug.Log("Hosting Game...");

        // Create a new relay
        if (!await RelayManager.CreateRelay(2)) {
            Debug.LogError("Failed to create relay.");
            return;
        }

        // Swap to main game scene
        Debug.Log($"Created Relay with code: {RelayManager.LobbyCode}");
        Instance._gameState.SessionId = $"{RelayManager.LobbyCode}";
        GameEventManager.TriggerOnGameStarted();
    }

    public async void JoinGame(string code) {
        Debug.Log("Joining Game...");

        // Join an existing relay
        if (!await RelayManager.JoinRelay(code)) {
            Debug.LogError("Failed to join relay.");
            GameEventManager.TriggerOnJoinRequestFailed();
            return;
        }

        // Swap to main game scene
        Debug.Log($"Joining Relay with code: {code}");
        GameEventManager.TriggerOnGameStarted();
    }

    public async void ExitGame()
    {
        RelayManager.Disconnect();
        _gameState.SessionId = "";
        Debug.Log($"Player disconnected");
    }
}
