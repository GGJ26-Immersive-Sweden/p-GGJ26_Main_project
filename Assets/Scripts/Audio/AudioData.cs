using UnityEngine;
using System.Collections.Generic;

public enum PitchType { VeryLow, Low, Normal, High, VeryHigh }

[System.Serializable]
public class AudioData {
    [SerializeField] string name;
    [SerializeField] bool onStart;
    [SerializeField] bool randomizeAudioClip;
    [SerializeField] List<AudioClip> clips = new();
    [SerializeField, Range(0f, 1f)] float volume = 1f;
    [SerializeField, Range(0f, 1f)] float randomVolumeRange = 0f;
    [SerializeField] PitchType basePitchType = PitchType.Normal;
    [SerializeField, Range(0f, 1f)] float randomPitchRange = 0f;
    [SerializeField] bool loop;

    public string Name { get => name; }
    public bool OnStart { get => onStart; }
    public bool RandomizeAudioClip { get => randomizeAudioClip; }
    public List<AudioClip> Clips { get => clips; }
    public float Volume { get => volume; }
    public float RandomPitchRange { get => randomPitchRange; }
    public float RandomVolumeRange { get => randomVolumeRange; }
    public bool Loop { get => loop;}

    public float Pitch {
        get {
            // Wow fancy
            return basePitchType switch
            {
                PitchType.VeryLow => .25f,
                PitchType.Low => .5f,
                PitchType.Normal => 1f,
                PitchType.High => 2f,
                PitchType.VeryHigh => 3f,
                _ => 0f,
            };
        }
    }

}