using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Global Game Jam 2021/Get Level Name")]
public class GetLevelName : MonoBehaviour
{
    [Header("Text Level")]
    [SerializeField] Text textToChange = default;
    [SerializeField] string beforeNameLevel = "Level: ";

    void OnEnable()
    {
        if (textToChange)
        {
            //find load level and get Name Level
            LoadLevelOnCollision loadLevelOnCollision = FindObjectOfType<LoadLevelOnCollision>();
            if (loadLevelOnCollision)
            {
                textToChange.text = beforeNameLevel + loadLevelOnCollision.NameLevel;
            }
        }
    }
}
