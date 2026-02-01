using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerSettings
{
   [SerializeField] private int playerID;
   [SerializeField] private Color color;
   [SerializeField] private LayerMask collision;
   [SerializeField] private LayerMask visibility;

    public readonly Color Color { get => color; }
    public readonly LayerMask Collision { get => collision; }
    public readonly LayerMask Visibility { get => visibility; }
}

[CreateAssetMenu(fileName = "GameState", menuName = "Scriptable Objects/GameState")]
public class GameState : ScriptableObject
{
    [SerializeField] private uint _score = 0;
    [SerializeField] private bool _players1Done = false;
    [SerializeField] private bool _players2Done = false;
    [SerializeField] private bool _completedLevel = false;
    [SerializeField] private int  _currentLevel = 0;
    
    private const uint MAX_PLAYERS = 2;
    
    // Networking
    private string _sessionId = "";
 
    // To store both players
    [SerializeField] public PlayerSettings[] _players = null;

    public uint Score 
    {
        get => _score; 
        private set => _score = value; 
    }

    public string SessionId 
    {
        get => _sessionId; 
        set => _sessionId = value; 
    }

#region API
    public void IncreaseScore(int amout)
    {
        if(amout <= 0) return;
        
        _score += (uint) amout;
    }

    public void ResetState()
    {
        _score = 0;
        _players = null;
    }

    public void PlayerDone(GameObject player = null)
    {
        if (player == null) return;

        if (player.layer == LayerMask.NameToLayer("Player 1")) {
            _players1Done = true;
            Debug.Log("Player 1 done!");
        } else if (player.layer == LayerMask.NameToLayer("Player 2")) {
            _players2Done = true;
            Debug.Log("Player 2 done!");
        } 
        
        if (_players1Done && _players2Done)
        {
            Level lvl = (Level)_currentLevel;
            Debug.Log("NextLevel Triggered");
            GameEventManager.TriggerOnLevelCompleted(lvl);
            _currentLevel++;

            _players1Done = false;
            _players2Done = false;
        }
    }

    internal void InitPlayer()
    {
        throw new NotImplementedException();
    }
#endregion

#region Private members
    private void Init()
    {
        //TODO Assign id and other stuff to the player
    }

#endregion

}
