using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] SoundScriptableObject sound;

    public void Play()
    {
        SoundPlayer.Instance.PlaySound(sound);
    }
}
