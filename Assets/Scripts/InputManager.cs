using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    // Component References
    private Rigidbody playerRigidbody;
    private NetworkPlayer networkPlayer;

    //[SerializeField] private GameObject _cosmeticPlayer;
    // Input values
    private bool jumpPressed = false;
    private float lastJumpPressedTime = -999f;
    private float lastGroundedTime = -999f;
    private Vector3 movementInput = Vector3.zero;
    private Vector3 movementValue = Vector3.zero;

    private GameObject walk_step_sfx = null;

    private bool grounded = false;

    [Header("Movement Parameters")]
    public float maxVelocity = 7.5f;
    public float jumpForce = 10f;

    [Header("Gravity")]
    public float fallGravityMultiplier = 2f;
    public float airtimeGravityMultiplier = 3f;
    public float variableJumpHeightGravityMultiplier = 100f;

    [Header("Jump Assist")]
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;
    public float variableJumpHeightBufferTime = 0.05f;

    void Start() {
        playerRigidbody = GetComponent<Rigidbody>();
        networkPlayer = GetComponent<NetworkPlayer>();
    }

    // Execute physics based movement input
    void FixedUpdate() {
        grounded = IsGrounded();

        if (grounded)
            lastGroundedTime = Time.time;

        if (CanJump())
            Jump();

        // Artificially Increase Gravity when in Air
        if (!grounded)
        {
            playerRigidbody.AddForce(Vector3.up * Physics.gravity.y * (airtimeGravityMultiplier - 1), ForceMode.Acceleration);
            
            // Artificially Increase Gravity when falling
            if (playerRigidbody.linearVelocity.y < 0)
            {
                playerRigidbody.AddForce(Vector3.up * Physics.gravity.y * (fallGravityMultiplier - 1), ForceMode.Acceleration);
            }
            else
            {
                // Increase Gravity if not holding down space (for variable jump height)
                if (Time.time - lastJumpPressedTime <= variableJumpHeightBufferTime)
                {
                    playerRigidbody.AddForce(Vector3.up * Physics.gravity.y * (variableJumpHeightGravityMultiplier - 1), ForceMode.Acceleration);
                }
            }
        }

        // Move
        movementValue = Vector3.Lerp(movementValue, movementInput, 0.1f);
        playerRigidbody.linearVelocity = new Vector3(
            movementValue.x * maxVelocity,
            playerRigidbody.linearVelocity.y,
            movementValue.z * maxVelocity
        );

        // Footstep audio
        if (movementValue.magnitude >= 1.0f)
        {
            Debug.Log("Runnin'");
            if (walk_step_sfx == null)
                (walk_step_sfx,_) = AudioManager.Instance.Play("Footstep");

            walk_step_sfx.GetComponent<AudioSource>().mute = false;
        }
        else
        {
            if (walk_step_sfx != null)
            {
                walk_step_sfx.GetComponent<AudioSource>().mute = true;
            }

        }
    }
    // Simple ground check using raycast
    bool IsGrounded() {
        Vector3 rayStart = transform.position;
        Vector3 rayDirection = Vector3.down;
        float rayLength = 1.1f; // Slightly longer than the player's height

        Debug.DrawRay(rayStart, rayDirection * rayLength, Color.red);

        return Physics.Raycast(rayStart, rayDirection, rayLength, networkPlayer.collidable);
    }
    
    // Set movement input from Input System
    void OnMove(InputValue value) {
        Vector2 movementVector = value.Get<Vector2>();

        // Rotate and store movement input based on camera orientation
        Quaternion rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        movementInput = rotation * new Vector3(movementVector.x, 0, movementVector.y);
    }

    void Jump()
    {
        // Reset any vertical velocity
        Vector3 velocity = playerRigidbody.linearVelocity;
        velocity.y = 0f;
        playerRigidbody.linearVelocity = velocity;

        AudioManager.Instance.Play("Jump");

        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Consume buffered jump
        lastJumpPressedTime = -999f;
        lastGroundedTime = -999f;

    }

    bool CanJump()
    {
        return
            Time.time - lastJumpPressedTime <= jumpBufferTime &&
            Time.time - lastGroundedTime <= coyoteTime;
    }

    // Set jump input from Input System 
    void OnJump() {  
        lastJumpPressedTime = Time.time; 
    } 

}
