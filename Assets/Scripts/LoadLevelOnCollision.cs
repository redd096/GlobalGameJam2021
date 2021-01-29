using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Load Level On Collision")]
public class LoadLevelOnCollision : MonoBehaviour
{
    [Header("Save Checkpoint")]
    [SerializeField] string nameCheckpoint = "nameThisLevel";

    [Header("Load Level")]
    [SerializeField] string nameLevelToLoad = "NameNextLevel";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if hit player and has head to end game
        Player player = collision.GetComponentInParent<Player>();
        if (player && player.CurrentHead.HeadToEndGame)
        {
            LoadNextLevel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit player and has head to end game
        Player player = collision.gameObject.GetComponentInParent<Player>();
        if (player && player.CurrentHead.HeadToEndGame)
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        //save checkpoint
        PlayerPrefs.SetInt(nameCheckpoint, 1);

        //load scene
        SceneLoader.instance.LoadNewScene(nameLevelToLoad);
    }
}
