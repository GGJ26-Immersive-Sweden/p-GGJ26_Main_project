using Unity.Netcode.Components;
using UnityEngine;

public class PlayerCosmeticUpdate : MonoBehaviour
{
    [SerializeField] private NetworkTransform _networkTransform;
    [SerializeField] private GameObject _cosmeticPlayer;

    // Store last frame position
    private Vector3 _lastPosition;
    private bool _hasLastPosition;

    void Update()
    {
        UpdateCosmeticPlayerOrientation();
    }

    private void UpdateCosmeticPlayerOrientation()
    {
        if (!_cosmeticPlayer || !_networkTransform)
            return;

        Vector3 currentPosition = _networkTransform.transform.position;

        if (!_hasLastPosition)
        {
            _lastPosition = currentPosition;
            _hasLastPosition = true;
            return;
        }

        Vector3 movementDelta = currentPosition - _lastPosition;

        Vector3 facingVector = new Vector3(movementDelta.x, 0f, movementDelta.z);

        if (facingVector.sqrMagnitude > 0.0001f)
        {
            _cosmeticPlayer.transform.forward = Vector3.Slerp(
                    _cosmeticPlayer.transform.forward,
                    facingVector.normalized,
                    Time.deltaTime * 10f
                );
        }

        _lastPosition = currentPosition;
    }
}
