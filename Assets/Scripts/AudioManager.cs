using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

public class AudioManager : Singleton<AudioManager>
{
    public static void PlaySound(GameObject go)
    {
        AudioSource audioSource = go.GetComponent<AudioSource>();
        if(audioSource)
        {
            audioSource.Play();
        }
    }
}
