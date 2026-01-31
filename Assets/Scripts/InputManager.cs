using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    // Component References
    private Rigidbody playerRigidbody;

    // Input values
    private bool jumpPressed = false;
    private Vector3 movementInput = Vector3.zero;
    private Vector3 movementValue = Vector3.zero;

    // Movement parameters
    public float maxVelocity = 7.5f;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Execute physics based movement input
    void FixedUpdate() {

        // Jump
        if (jumpPressed) {
            playerRigidbody.AddForce(Vector3.up * 7.5f, ForceMode.Impulse);
            jumpPressed = false;
        }

        // Move
        movementValue = Vector3.Lerp(movementValue, movementInput, 0.1f);
        playerRigidbody.linearVelocity = new Vector3(
            movementValue.x * maxVelocity,
            playerRigidbody.linearVelocity.y,
            movementValue.z * maxVelocity
        );
    }
    
    // Simple ground check using raycast
    bool IsGrounded() {
        Vector3 rayStart = transform.position;
        Vector3 rayDirection = Vector3.down;
        float rayLength = 1.1f; // Slightly longer than the player's height

        Debug.DrawRay(rayStart, rayDirection * rayLength, Color.red);

        return Physics.Raycast(rayStart, rayDirection, rayLength);
    }
    
    // Set movement input from Input System
    void OnMove(InputValue value) {
        Vector2 movementVector = value.Get<Vector2>();

        // Rotate and store movement input based on camera orientation
        Quaternion rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        movementInput = rotation * new Vector3(movementVector.x, 0, movementVector.y);
    }

    // Set jump input from Input System
    void OnJump() {
        if (IsGrounded()) {
            jumpPressed = true;
        }
    }
}
