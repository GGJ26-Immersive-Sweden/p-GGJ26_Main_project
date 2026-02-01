using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CollectableComponent : MonoBehaviour
{
    [SerializeField] TagHandle playerTag;
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

        if (!other.CompareTag("Player"))
            return;

        StartCoroutine(nameof(PlayCollectEffect));

        gameState.PlayerDone();
    }

    IEnumerator PlayCollectEffect()
    {
        Debug.Log("Key Effect!");

        meshRenderer.enabled = false;
        hitBox.enabled = false;
        AudioClip clip;
        (_,clip) = AudioManager.Instance.Play("CollectWin");
        ps?.Play();
        yield return _waitForSeconds1_0;
        ps?.Stop();
        Destroy(this);
    }
}
