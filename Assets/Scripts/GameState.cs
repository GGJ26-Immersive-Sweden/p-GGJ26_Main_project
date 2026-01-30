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
    private const uint MAX_PLAYERS = 2;

    // To store both players
    private List<Player> _players = null;

    public uint Score 
    {
        get => _score; 
        private set => _score = value; 
    }

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

}
