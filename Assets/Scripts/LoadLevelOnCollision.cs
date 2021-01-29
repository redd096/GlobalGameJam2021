using UnityEngine;
using redd096;

[AddComponentMenu("Global Game Jam 2021/Load Level On Collision")]
public class LoadLevelOnCollision : MonoBehaviour
{
    [Header("Load Level")]
    [SerializeField] string nameLevelToLoad = "NameLevel";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if hit player
        if(collision.GetComponentInParent<Player>())
        {
            LoadLevel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit player
        if (collision.gameObject.GetComponentInParent<Player>())
        {
            LoadLevel();
        }
    }

    void LoadLevel()
    {
        //load scene
        SceneLoader.instance.LoadNewScene(nameLevelToLoad);
    }
}
