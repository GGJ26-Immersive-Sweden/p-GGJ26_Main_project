using UnityEngine;
using System.Collections;
using System;

public class AudioManager : Singleton<AudioManager> {

    protected override void Awake() => base.Awake();
    [SerializeField] AudioManagerData data;

    GameObject coop;

    // Start all clips labelled OnStart (e.g. Music)
    void Start() {
        foreach (var audioData in data.Datas) {
            if (audioData.OnStart) {
                Play(audioData.Name);
            }
        }
    }

    // Call from other classes using the "audio-clip name" to play the audio-clip
    // If you want to stop a looping audio just hold its gameobject handle and call mute on the audiosource
    public (GameObject gameObject, AudioClip audioClip) Play(string name) {
        foreach (var audioData in data.Datas) {
            if (audioData.Name == name) {
                return Play(audioData);
            }
        }
        return (null, null);
    }

    public (GameObject gameObject, AudioClip audioClip) PlayAtLocation(string name, Vector3 location) {
        foreach (var audioData in data.Datas) {
            if (audioData.Name == name) {
                return Play(audioData);
            }
        }

        return (null, null);
    }

    public (GameObject gameObject, AudioClip audioClip) Play(AudioData data, Action onDestroy = null, Vector3? soundPosition = null) {
        var res = (go: null as GameObject, ac: null as AudioClip);

        var coroutine = PlayIE(data, onDestroy, (go, ac) => {
            res.go = go; res.ac = ac;
        }, soundPosition);

        StartCoroutine(coroutine);

        return res;
    }

    IEnumerator PlayIE(AudioData data, Action onDestroy, Action<GameObject, AudioClip> result, Vector3? soundPosition = null) {
        var go = new GameObject(data.Name);
        var audioSource = go.AddComponent<AudioSource>();

        if (soundPosition.HasValue)
            go.transform.position = soundPosition.Value;

        do
        {
            int selectedClip = UnityEngine.Random.Range(0, data.Clips.Count);
            audioSource.clip = data.Clips[selectedClip];

            audioSource.volume = data.Volume +
                (data.RandomVolumeRange != 0f
                    ? UnityEngine.Random.Range(-data.RandomVolumeRange / 2f, data.RandomVolumeRange / 2f)
                    : 0f);

            audioSource.pitch = data.Pitch +
                (data.RandomPitchRange != 0f
                    ? UnityEngine.Random.Range(-data.RandomPitchRange / 2f, data.RandomPitchRange / 2f)
                    : 0f);

            audioSource.Play();

            result?.Invoke(go, audioSource.clip);

            if (audioSource.mute) break; 

            yield return new WaitForSeconds(audioSource.clip.length);

        } while (data.Loop);

        Destroy(go);

        onDestroy?.Invoke();
    }
}