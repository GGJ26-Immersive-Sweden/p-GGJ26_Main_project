using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Scriptable Objects/GameState")]
public class GameState : ScriptableObject
{
    [SerializeField] private uint _score = 0;

    public uint Score 
    {
        get => _score; 
        set => _score = value; 
    }
    
}
