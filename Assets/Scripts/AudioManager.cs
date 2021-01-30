using UnityEngine;
using redd096;


public class AudioManager : Singleton<AudioManager>
{
    public static void PlaySound(GameObject go)
    {
        AudioSource audioSource = go.GetComponentInChildren<AudioSource>();
        if (audioSource)
        {
            audioSource.Play();
        }
    }
}