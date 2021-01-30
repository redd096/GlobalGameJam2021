namespace redd096
{
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [AddComponentMenu("redd096/Singletons/Game Manager")]
    public class GameManager : Singleton<GameManager>
    {
        [Header("Tombstone")]
        [SerializeField] int limitTombStones = 5;
        [SerializeField] Tombstone tombStonePrefab = default;

        int previousScene;
        List<Vector3> deathPositions = new List<Vector3>();
        Coroutine spawnTombStonesCoroutine;

        public UIManager uiManager { get; private set; }

        protected override void SetDefaults()
        {

            //get references
            uiManager = FindObjectOfType<UIManager>();

            CheckSceneAndSpawnTombstones();

            //ci sarà un sound manager che ogni volta che attiva un suono checka gli slider dei volumi

            //attiva e disattiva post process

            //attiva e disattiva la visuale col mouse
        }

        #region tombstones

        void CheckSceneAndSpawnTombstones()
        {
            //if same level, instantiate at every death position
            if (previousScene == SceneManager.GetActiveScene().buildIndex)
            {
                if (spawnTombStonesCoroutine != null)
                    StopCoroutine(spawnTombStonesCoroutine);

                StartCoroutine(SpawnTombStonesCoroutine());
            }
            //else clear list when change level
            else
            {
                deathPositions.Clear();
            }

            //save previous scene
            previousScene = SceneManager.GetActiveScene().buildIndex;
        }

        IEnumerator SpawnTombStonesCoroutine()
        {
            foreach (Vector3 position in deathPositions)
            {
                Instantiate(tombStonePrefab, position, Quaternion.identity);
                yield return null;
            }
        }

        #endregion

        #region public API

        public void EndLevel(Vector3 position)
        {
            //add to list
            deathPositions.Add(position);

            //if reached limit, remove oldest one
            if(deathPositions.Count >= limitTombStones)
            {
                deathPositions.RemoveAt(0);
            }
        }

        #endregion
    }
}