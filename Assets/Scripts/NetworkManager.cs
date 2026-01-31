using System;
using UnityEngine;

public class NetworkManager : Singleton<NetworkManager>
{
    public static event Action OnSessionCreated;
    public static event Action OnSessionJoined;
    public static event Action OnSessionClose;

    protected override void Awake() => base.Awake();
    protected override void OnDestroy() => base.OnDestroy();

    void OnDisable()
    {
        OnSessionCreated = null;
        OnSessionJoined = null;
        OnSessionClose = null;
    }


#region API
    public static void CreateSession()
    {
        //TODO call the async function here

        //TODO once it resolves call:
        OnSessionCreated?.Invoke();
    }
    public static void JoinSession()
    {
        //TODO call the async function here
        
        //TODO once it resolves call:
        OnSessionJoined?.Invoke();
    }
    public static void CloseSession()
    {
        //TODO call the async function here
        
        //TODO once it resolves call:
        OnSessionClose?.Invoke();
    }
#endregion
}