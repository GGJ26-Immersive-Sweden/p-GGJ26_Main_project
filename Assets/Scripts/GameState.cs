using System;
using System.Collections.Generic;
using UnityEngine;

struct Player
{
   uint color;
}

[CreateAssetMenu(fileName = "GameState", menuName = "Scriptable Objects/GameState")]
public class GameState : ScriptableObject
{
    [SerializeField] private uint _score = 0;
    [SerializeField] private uint _playersDone = 0;
    [SerializeField] private bool _completedLevel = false;
    [SerializeField] private int  _currentLevel = 0;
    
    private const uint MAX_PLAYERS = 2;
    
    // Networking
    private string _sessionId = "";

    // To store both players
    private List<Player> _players = null;

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
        _players.Clear();
        _players = null;
    }

    public void PlayerDone()
    {
        _playersDone++;
        if (_playersDone >= 2)
        {
            Level lvl = (Level)_currentLevel;
            GameEventManager.TriggerOnLevelCompleted(lvl);
        }
        _playersDone = 0;
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
