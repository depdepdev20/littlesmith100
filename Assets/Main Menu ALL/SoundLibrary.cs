using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundEffect
{
    public string groupID;
    public AudioClip[] clips; // Fixed declaration
}

public class SoundLibrary : MonoBehaviour // Changed from struct to class
{
    public SoundEffect[] soundEffects;

    public AudioClip GetClipFromName(string name)
    {
        foreach (var soundEffect in soundEffects)
        {
            if (soundEffect.groupID == name)  
            {
                // Corrected the indexing logic
                return soundEffect.clips[Random.Range(0, soundEffect.clips.Length)];
            }
        }
        return null; // Return null if no match is found
    }
}
