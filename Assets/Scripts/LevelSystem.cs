using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct LevelStruct
{
    public Button button;
    public string checkPointNecessary;
}

public class LevelSystem : MonoBehaviour
{
    [SerializeField] LevelStruct[] levels = default;

    void Start()
    {
        //foreach level, active or deactive
        foreach(LevelStruct level in levels)
        {
            //if reached checkpoint, active button
            if(level.checkPointNecessary == string.Empty || PlayerPrefs.GetInt(level.checkPointNecessary, 0) > 0)
            {
                level.button.interactable = true;
            }
            //else deactive
            else
            {
                level.button.interactable = false;
            }
        }
    }
}
