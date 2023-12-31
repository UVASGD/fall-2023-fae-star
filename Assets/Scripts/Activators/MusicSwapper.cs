using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwapper : MonoBehaviour, IActivator
{
    [SerializeField] AudioSource[] songs;
    [SerializeField] int transitionLengthFrames;
    [SerializeField] float targetVolume;

    
    private int selectedSong;
    
    // Start is called before the first frame update
    void Awake()
    {
        selectedSong = 0;
        foreach (AudioSource s in songs)
        {
            s.volume = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < songs.Length; i++)
        {
            if(i == selectedSong && songs[i].volume < 1f * targetVolume)
            {
                songs[i].volume += (1f * targetVolume) / transitionLengthFrames;
            }
            else if(songs[i].volume > 0)
            {
                songs[i].volume -= 1f / transitionLengthFrames;
            }
        }
    }

    public void Activate(int activationStyle, int source)
    {
        if(activationStyle < songs.Length)
            selectedSong = activationStyle;
    }

    public void Deactivate(int activationStyle, int source)
    {
        // DO NOTHING
    }
}
