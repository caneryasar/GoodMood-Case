using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/AudioData")]

public class AudioData : ScriptableObject {

    public AudioClip walkSound;
    public List<AudioClip> attackSounds; 
    public List<AudioClip> hitSounds; 
    public AudioClip dummyDeath;
}