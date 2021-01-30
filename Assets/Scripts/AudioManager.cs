using UnityEngine;
using redd096;


[AddComponentMenu("Global Game Jam 2021/Audio Manager")]
public class AudioManager : Singleton<AudioManager>
{
    public static void PlaySound(GameObject go, AudioClip[] audioClip)
    {
        //set audio clip and play
        AudioSource audioSource = go.GetComponentInChildren<AudioSource>();
        if (audioSource && audioClip != null && audioClip.Length > 0)
        {
            audioSource.clip = audioClip[Random.Range(0, audioClip.Length)];
            audioSource.Play();
        }
    }
}