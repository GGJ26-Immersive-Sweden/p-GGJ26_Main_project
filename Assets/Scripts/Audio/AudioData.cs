using UnityEngine;
using System.Collections.Generic;

public enum PitchType { VeryLow, Low, Normal, High, VeryHigh }

[System.Serializable]
public class AudioData {
    [SerializeField] string name;
    [SerializeField] List<AudioClip> clips = new();
    [SerializeField, Range(0f, 1f)] float volume = 1f;
    [SerializeField] PitchType basePitchType = PitchType.Normal;
    [SerializeField] bool onStart;
    [SerializeField] bool loop;
    [SerializeField] bool randomizeAudioClip;
    [SerializeField, Range(0f, 1f)] float randomVolumeRange = 0f;
    [SerializeField, Range(0f, 1f)] float randomPitchRange = 0f;

    public string Name { get => name; }
    public List<AudioClip> Clips { get => clips; }
    public bool RandomizeAudioClip { get => randomizeAudioClip; }
    public float Volume { get => volume; }
    public bool OnStart { get => onStart; }
    public bool Loop { get => loop;}
    public float RandomVolumeRange { get => randomVolumeRange; }
    public float RandomPitchRange { get => randomPitchRange; }
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