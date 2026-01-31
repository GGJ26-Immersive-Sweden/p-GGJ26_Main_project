using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This is the interface to communicate any Editor-base event with the
/// Global GameEventManager Singleton
/// </summary>
public class GuiController : MonoBehaviour
{

    public void TriggerJoinSessionButton() { if (GameEventManager.Instance != null) GameEventManager.TiggerOnJoinRequested(); }
    public void TriggerWinGameButton() { if (GameEventManager.Instance != null) GameEventManager.TriggerOnGameWon(); }
    public void TriggerGameOverGameButton() { if (GameEventManager.Instance != null) GameEventManager.TriggerOnGameOver(); }
    public void TriggerExitGameButton() { if (GameEventManager.Instance != null) GameEventManager.TriggerOnExitRequested(); }

}