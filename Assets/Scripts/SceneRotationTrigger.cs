using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SceneRotationTrigger : MonoBehaviour
{
    private BoxCollider _collider;
    [SerializeField] private CameraRotationController _cameraRotationController;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO validate that the player (me) collides with the trigger

        if(_cameraRotationController != null)
            _cameraRotationController.RotateCameraToNextRoom();


        gameObject.SetActive(false);
    }
}
