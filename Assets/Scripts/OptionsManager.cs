using UnityEngine;
using redd096;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[AddComponentMenu("Global Game Jam 2021/Options Manager")]
public class OptionsManager : Singleton<OptionsManager>
{
    [Header("UI")]
    [SerializeField] Slider sliderVolume = default;
    [SerializeField] Toggle toggleUseAim = default;
    [SerializeField] Toggle toggleUsePostProcessLayer = default;

    float volume;
    bool useAim;
    bool usePostProcessLayer;

    void OnEnable()
    {
        //load 
        volume = PlayerPrefs.GetFloat("Options_Volume", sliderVolume.value);
        useAim = PlayerPrefs.GetInt("Options_UseAim", toggleUseAim.isOn ? 1 : 0) > 0 ? true : false;
        usePostProcessLayer = PlayerPrefs.GetInt("Options_UsePostProcess", toggleUsePostProcessLayer.isOn ? 1 : 0) > 0 ? true : false;

        //update UI when load
        sliderVolume.value = volume;
        toggleUseAim.isOn = useAim;
        toggleUsePostProcessLayer.isOn = usePostProcessLayer;
    }

    protected override void SetDefaults()
    {
        base.SetDefaults();

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
        volume = newVolume;
        PlayerPrefs.SetFloat("Options_Volume", newVolume);

        //set in scene
        SetInScene();
    }

    public void SetUseMouse(bool useMouse)
    {
        //save
        useAim = useMouse;
        PlayerPrefs.SetInt("Options_UseAim", useMouse ? 1 : 0);

        //set in scene
        SetInScene();
    }

    public void SetPostProcess(bool usePostProcess)
    {
        //save
        usePostProcessLayer = usePostProcess;
        PlayerPrefs.SetInt("Options_UsePostProcess", usePostProcess ? 1 : 0);

        //set in scene
        SetInScene();
    }

    #endregion
}
