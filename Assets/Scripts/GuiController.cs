using UnityEngine;

public class GuiController : MonoBehaviour
{
    public void TriggerJoinSessionButton()
    {
        GameEventManager.TiggerOnJoinRequested();
    }
    
}