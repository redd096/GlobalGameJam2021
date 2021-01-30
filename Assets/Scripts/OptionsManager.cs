using UnityEngine;
using redd096;
using UnityEngine.Rendering.PostProcessing;

[AddComponentMenu("Global Game Jam 2021/Options Manager")]
public class OptionsManager : Singleton<OptionsManager>
{
    [Header("Default")]
    [Range(0f, 1f)] [SerializeField] float defaultVolume = 1;
    [SerializeField] bool defaultUseAim = false;
    [SerializeField] bool defaultUsePostProcess = true;

    public float volume { get; private set; }
    public bool useAim { get; private set; }
    public bool usePostProcessLayer { get; private set; }

    protected override void SetDefaults()
    {
        base.SetDefaults();

        //load 
        volume = PlayerPrefs.GetFloat("Options_Volume", defaultVolume);
        useAim = PlayerPrefs.GetInt("Options_UseAim", defaultUseAim ? 1 : 0) > 0 ? true : false;
        usePostProcessLayer = PlayerPrefs.GetInt("Options_UsePostProcess", defaultUsePostProcess ? 1 : 0) > 0 ? true : false;

        //set in scene
        SetInScene();
    }

    void SetInScene()
    {
        //set volume
        AudioListener.volume = volume;

        //set aim
        Player player = FindObjectOfType<Player>();
        if(player)
        {
            player.useAim = useAim;
        }

        //set post process
        Camera cam = Camera.main;
        if (cam)
        {
            PostProcessLayer post = cam.GetComponent<PostProcessLayer>();
            if(post)
            {
                post.enabled = usePostProcessLayer;
            }
        }
    }

    #region public API

    public void SetVolume(float newVolume)
    {
        //save
        instance.volume = newVolume;
        PlayerPrefs.SetFloat("Options_Volume", newVolume);

        //set in scene
        instance.SetInScene();
    }

    public void SetUseMouse(bool useMouse)
    {
        //save
        instance.useAim = useMouse;
        PlayerPrefs.SetInt("Options_UseAim", useMouse ? 1 : 0);

        //set in scene
        instance.SetInScene();
    }

    public void SetPostProcess(bool usePostProcess)
    {
        //save
        instance.usePostProcessLayer = usePostProcess;
        PlayerPrefs.SetInt("Options_UsePostProcess", usePostProcess ? 1 : 0);

        //set in scene
        instance.SetInScene();
    }

    #endregion
}
