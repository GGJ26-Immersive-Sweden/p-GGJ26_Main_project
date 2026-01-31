using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CollectableComponent : MonoBehaviour
{
    [SerializeField] private TagHandle playerTag;
    [SerializeField] GameState gameState; 

    [SerializeField] ParticleSystem ps; 
    [SerializeField] MeshRenderer meshRenderer; 
    [SerializeField] Collider hitBox; 

    // Fancy :O ~
    private static WaitForSeconds _waitForSeconds1_0 = new(1.0f);

    private void Start()
    {
        TryGetComponent<ParticleSystem>(out ps);
        TryGetComponent<MeshRenderer>(out meshRenderer);
        TryGetComponent<Collider>(out hitBox);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        // Collected, Level done!
        Debug.Log("Key Collected");
        StartCoroutine("PlayCollectEffect");

        gameState.PlayerDone();
    }

    IEnumerator PlayCollectEffect()
    {
        meshRenderer.enabled = false;
        hitBox.enabled = false;
        AudioClip clip;
        (_,clip) = AudioManager.Instance.Play("collect_win");
        ps?.Play();
        yield return _waitForSeconds1_0;
        ps?.Stop();
        Destroy(this);
    }
}
