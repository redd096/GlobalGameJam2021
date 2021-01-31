using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Global Game Jam 2021/Options UI")]
public class OptionsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Slider sliderVolume = default;
    [SerializeField] Toggle toggleUseAim = default;
    [SerializeField] Toggle toggleUsePostProcessLayer = default;
    [SerializeField] Toggle toggleFullScreen = default;

    void OnEnable()
    {
        //update UI
        sliderVolume.value = OptionsManager.instance.volume;
        toggleUseAim.isOn = OptionsManager.instance.useAim;
        toggleUsePostProcessLayer.isOn = OptionsManager.instance.usePostProcessLayer;
        toggleFullScreen.isOn = OptionsManager.instance.useFullScreen;
    }
}
