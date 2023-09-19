using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [Header("Runtime")]
    [SerializeField] bool debug = false;


    //Singleton
    static public SoundPlayer Instance { get; private set; }
    private AudioSource source;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
    }

    //This can be called from anywhere with AudioPlayer.Instance.PlaySound
    //Use primarily for fx, ui, etc. instead of music
    public void PlaySound(AudioClip clip, float volume, bool loop = false)
    {
        if(loop) source.loop = true;
        else source.loop = false;

        source.PlayOneShot(clip, volume);

        if(debug) Debug.Log("played sound " + clip.name + " at volume " + volume + " with loop " + (loop ? "on.":"off."));
    }

    //This can be called from anywhere with AudioPlayer.Instance.PlaySound
    //Use primarily for fx, ui, etc. instead of music
    public void PlaySound(SoundScriptableObject soundScriptableObject)
    {
        if(soundScriptableObject.loop) source.loop = true;
        else source.loop = false;

        source.PlayOneShot(soundScriptableObject.clip, soundScriptableObject.volume);

        if(debug) Debug.Log("played sound " + soundScriptableObject.clip.name + " at volume " + soundScriptableObject.volume + " with loop " + (soundScriptableObject.loop ? "on.":"off."));
    }
}
