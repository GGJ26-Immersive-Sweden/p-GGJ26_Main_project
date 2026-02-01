using Unity.Netcode.Components;
using UnityEngine;

public class PlayerCosmeticUpdate : MonoBehaviour
{
   [SerializeField] private NetworkTransform _networkTransform;
       [SerializeField] private GameObject _cosmeticPlayer;

    void Update()
    {
        UpdateCosmeticPlayerOrientation();
    }

    private void UpdateCosmeticPlayerOrientation()
{
    if (_cosmeticPlayer && _networkTransform)
    {
        // Get the forward direction from the NetworkTransform's transform
        Vector3 forward = _networkTransform.transform.forward;
        
        // Flatten to horizontal plane (ignore vertical tilt)
        Vector3 facingVector = new Vector3(forward.x, 0f, forward.z).normalized;
        
        // Only update if we have a valid direction
        if (facingVector.sqrMagnitude > 0.01f)
        {
            _cosmeticPlayer.transform.forward = facingVector;
        }
    }
}
}
