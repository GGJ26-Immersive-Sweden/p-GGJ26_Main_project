using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour {

    // Component references
    private Rigidbody rb;
    private ParticleSystem ps;

    // Respawn point for the player
    [SerializeField] private Transform respawnPoint;

    // Filter collisions based on layer mask
    [SerializeField] private LayerMask collidableWith;
    public LayerMask collidable {
        get { return collidableWith; }
        set {
            UpdateCollidableLayerMask(value);
            collidableWith = value;
        }
    }

    // Layers to filter visibility
    [SerializeField] private LayerMask canSee;
    public LayerMask visible {
        get { return canSee; }
        set {
            UpdateVisibilityLayerMask(value);
            canSee = value;
        }
    }

    // Initialize component references
    void Awake() {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
    }

    // Update inspectors changes
    void OnValidate() {
        UpdateCollidableLayerMask(collidableWith);
        UpdateVisibilityLayerMask(canSee);
    }

    // Handle trigger events
    void OnTriggerEnter(Collider collider) {
        switch (collider.gameObject.tag) {
            case "Death Zone":
                RespawnRpc();
                break;
            case "Goal":
                Debug.Log("Player reached the goal!");
                break;
        }
    }

    // Updates collider matrix based on layer mask
    void UpdateCollidableLayerMask(LayerMask mask) {
        for (int i = 0; i < 32; i++) {
            Physics.IgnoreLayerCollision(this.gameObject.layer, i, (mask.value & (1 << i)) == 0);
        }
    }

    // Updates visibility based on layer mask
    void UpdateVisibilityLayerMask(LayerMask mask) {
        Camera.main.cullingMask = mask;

        // Get all lights in the scene and update their culling masks
        Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light light in lights) {
            UniversalAdditionalLightData lightData = light.GetComponent<UniversalAdditionalLightData>();
            RenderingLayerMask lightLayers = lightData.renderingLayers;
            lightData.renderingLayers = 0;

            // If layer is not in mask, change rendering layer to nothing
            if ((mask.value & (1 << light.gameObject.layer)) == 0) {
                lightData.renderingLayers = 0;

            // Else, change rendering layer to default
            } else {
                lightData.renderingLayers = 1;
            }
        }
    }

    // Respawn the player at respawn point with delay
    [Rpc(SendTo.Everyone)]
    public void RespawnRpc() {
        Debug.Log("Player Respawning...");

        // Trigger death particle effect, sound and make invisible
        ps?.Play();
        //SoundManager.PlaySoundAtPosition("PlayerDeath", this.transform.position);
        this.gameObject.SetActive(false);

        // Reset velocity and position
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        this.transform.position = respawnPoint.position;
        this.transform.rotation = respawnPoint.rotation;
        this.transform.localScale = respawnPoint.localScale;

        // Make player visible again
        this.gameObject.SetActive(true);
    }
}
