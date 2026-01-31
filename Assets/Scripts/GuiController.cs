using UnityEngine;

/// <summary>
/// This is the interface to communicate any Editor-base event with the
/// Global GameEventManager Singleton
/// </summary>
public class GuiController : MonoBehaviour
{
    public void TriggerJoinSessionButton() => GameEventManager.TiggerOnJoinRequested();
    public void TriggerWinGameButton() => GameEventManager.TriggerOnGameWon();
    public void TriggerGameOverGameButton() => GameEventManager.TriggerOnGameOver();
    public void TriggerExitGameButton() => GameEventManager.TriggerOnExitRequested();
    
}