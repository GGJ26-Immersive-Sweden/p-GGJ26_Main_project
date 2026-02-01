using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{

    // Component references
    private Rigidbody rb;
    private ParticleSystem ps;
    private MeshRenderer ms;
    [SerializeField] private CameraRotationController cameraRotationController;

    // Respawn point for the player
    [SerializeField] private Vector3 respawnPoint = Vector3.zero;
    [SerializeField] private GameState gameState;

    // Filter collisions based on layer mask
    [SerializeField] private LayerMask collidableWith;
    public LayerMask collidable
    {
        get { return collidableWith; }
        set
        {
            UpdateCollidableLayerMask(value);
            collidableWith = value;
        }
    }

    // Layers to filter visibility
    [SerializeField] private LayerMask canSee;
    public LayerMask visible
    {
        get { return canSee; }
        set
        {
            UpdateVisibilityLayerMask(value);
            canSee = value;
        }
    }

    // Initialize component references
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
        ms = GetComponent<MeshRenderer>();

        SetupPlayerAttributesRpc((int)OwnerClientId);
    }


    // Update inspectors changes
    void OnValidate()
    {
        UpdateCollidableLayerMask(collidableWith);
        UpdateVisibilityLayerMask(canSee);
    }

    // Handle trigger events
    void OnTriggerEnter(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Death Zone":
                RespawnRpc();
                break;
            case "Goal":
                GameEventManager.Instance._gameState.PlayerDone(this.gameObject);
                break;
            case "Door Entry":
                if (IsOwner) {
                    respawnPoint = collider.transform.position;
                    Camera.main.GetComponent<CameraRotationController>().RotateCameraToNextRoom(new Vector3(12.8f, 0f, 13.2f), -90f, 0.5f);
                    collider.gameObject.SetActive(false);
                }
                break;
        }
    }

    // Updates collider matrix based on layer mask
    void UpdateCollidableLayerMask(LayerMask mask)
    {
        for (int i = 0; i < 32; i++)
        {
            Physics.IgnoreLayerCollision(this.gameObject.layer, i, (mask.value & (1 << i)) == 0);
        }
    }

    // Updates visibility based on layer mask
    void UpdateVisibilityLayerMask(LayerMask mask)
    {
        if (!IsOwner) return;

        Camera.main.cullingMask = mask;

        // Get all lights in the scene and update their culling masks
        Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light light in lights)
        {
            UniversalAdditionalLightData lightData = light.GetComponent<UniversalAdditionalLightData>();
            RenderingLayerMask lightLayers = lightData.renderingLayers;
            lightData.renderingLayers = 0;

            // If layer is not in mask, change rendering layer to nothing
            if ((mask.value & (1 << light.gameObject.layer)) == 0)
            {
                lightData.renderingLayers = 0;

                // Else, change rendering layer to default
            }
            else
            {
                lightData.renderingLayers = 1;
            }
        }
    }

    // Respawn the player at respawn point with delay
    [Rpc(SendTo.Everyone)]
    public void RespawnRpc()
    {
        Debug.Log("Player Respawning...");

        // Reset velocity and position
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        this.transform.position = respawnPoint;
        this.transform.localScale = Vector3.zero;

        // Make player visible again
        StartCoroutine(ScaleOverTime(Vector3.one, 0.5f));
    }

    public IEnumerator ScaleOverTime(Vector3 targetScale, float duration)
    {
        yield return new WaitForSeconds(0.25f); // Small delay before starting the scaling

        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;
        while ((elapsedTime += Time.deltaTime) < duration) {
            // Optional: Apply easing for smoother feel (ease-in-out)
            float t = elapsedTime / duration;
            float easedT = t * t * (3f - 2f * t);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, easedT);
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public void SetupPlayerAttributesRpc(int id)
    {
        if (id > gameState._players.Length)
            Debug.LogError("Not Enough Player Settings!");

        if (0 == id) {
            gameObject.layer = LayerMask.NameToLayer("Player 1");
        } else if (1 == id) {
            gameObject.layer = LayerMask.NameToLayer("Player 2");
        }
        
        UpdateCollidableLayerMask(gameState._players[id].Collision);
        UpdateVisibilityLayerMask(gameState._players[id].Visibility);
        ms.material.color = gameState._players[id].Color;
    }

    public override void OnNetworkSpawn()
    {
        SetupPlayerAttributesRpc((int)OwnerClientId);

        Debug.Log($"[SPAWN] Player spawned. IsOwner: {IsOwner}, ClientId: {OwnerClientId}");
        Debug.Log($"[SPAWN] Position: {transform.position}");
        Debug.Log($"[SPAWN] Layer: {LayerMask.LayerToName(gameObject.layer)}");
        Debug.Log($"[SPAWN] Has Collider: {GetComponent<Collider>() != null}");
        Debug.Log($"[SPAWN] Rigidbody isKinematic: {rb.isKinematic}");

        GetComponent<InputManager>().enabled = IsOwner;
        GetComponent<PlayerInput>().enabled = IsOwner;
    }

}
