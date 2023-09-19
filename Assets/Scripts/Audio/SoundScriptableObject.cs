using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundScriptableObject", menuName = "Sound/SoundScriptableObject")]
public class SoundScriptableObject : ScriptableObject
{
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = 0.5f;
    public bool loop = false;
}
