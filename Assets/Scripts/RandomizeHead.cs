using UnityEngine;

[AddComponentMenu("Global Game Jam 2021/RandomizeHead")]
public class RandomizeHead : MonoBehaviour
{
    [Header("Heads")]
    [SerializeField] SpriteRenderer spriteToChange = default;
    [SerializeField] Sprite[] heads = default;

    [Header("Golden Head")]
    [SerializeField] Sprite goldenHead = default;
    [Range(0, 1)] [SerializeField] float percentageGolden = 0.1f;

    void Start()
    {
        Sprite sprite;

        float random = Random.value;
        //if percentage, take golden (only if there is golden)
        if(random <= percentageGolden && goldenHead != null)
        {
            sprite = goldenHead;
        }
        //else random head
        else
        {
            sprite = heads[Random.Range(0, heads.Length)];
        }

        //change sprite if there is one
        if(sprite != null)
            spriteToChange.sprite = sprite;
    }
}
