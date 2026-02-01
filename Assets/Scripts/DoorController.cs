using System;
using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject _doorGameObject;

    void OnEnable() => GameEventManager.OnLevelCompleted += OnLevelCompletedHandler;
    void OnDisable() => GameEventManager.OnLevelCompleted -= OnLevelCompletedHandler;
    
    private void OnLevelCompletedHandler(Level level)
    {
        StartCoroutine(SlideDoorUpDown(5));
    }

    public void TriggerDoorDown()
    {
        StartCoroutine(SlideDoorUpDown(5));
    }

    private IEnumerator SlideDoorUpDown(float startTime)
    {
        float current = startTime;

        while(current > 0f)
        {
            current -= 1f * Time.deltaTime;
            transform.position += Vector3.down * 1.5f * Time.deltaTime;
            yield return null;
        }

    }
}
