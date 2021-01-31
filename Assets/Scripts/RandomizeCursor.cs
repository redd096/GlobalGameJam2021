using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Randomize Cursor")]
public class RandomizeCursor : Singleton<RandomizeCursor>
{
    [Header("Cursors")]
    [SerializeField] Texture2D[] cursors = default;

    [Header("Golden Cursor")]
    [SerializeField] Texture2D goldenCursor = default;
    [Range(0, 1)] [SerializeField] float percentageGolden = 0.01f;

    protected override void SetDefaults()
    {
        base.SetDefaults();

        Texture2D sprite;

        float random = Random.value;
        //if percentage, take golden (only if there is golden)
        if (random <= percentageGolden && goldenCursor != null)
        {
            sprite = goldenCursor;
        }
        //else random head
        else
        {
            sprite = cursors[Random.Range(0, cursors.Length)];
        }

        //change sprite if there is one
        Cursor.SetCursor(sprite, Vector2.zero, CursorMode.Auto);
    }
}
