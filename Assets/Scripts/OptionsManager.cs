using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Options Manager")]
public class OptionsManager : Singleton<OptionsManager>
{
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetUseMouse(bool useMouse)
    {

    }

    public void SetPostProcess(bool usePostProcess)
    {

    }
}
