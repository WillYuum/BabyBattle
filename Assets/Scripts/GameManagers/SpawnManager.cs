using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;
using Utils.ArrayUtils;
using GameplayUtils.Prefabs;

namespace SpawnManagerCore
{
    public class SpawnManager : MonoBehaviourSingleton<SpawnManager>
    {
        [SerializeField] private PseudoRandArray<Transform> _enemyTroopSpawnPoints;


        [SerializeField] private PrefabConfig _bigBabyPrefabConfig;

        [SerializeField] private float spawnDelay = 3.0f;


        void Awake()
        {
            GameloopManager.instance.OnGameLoopStarted += () => ToggleSpawnEnemy(true);
            GameloopManager.instance.OnLoseGame += () => ToggleSpawnEnemy(false);
        }



        private void ToggleSpawnEnemy(bool canSpawn)
        {
            if (canSpawn)
            {
                Debug.Log("Started spawning enemies");
                StartCoroutine(SpawnEnemies());
            }
            else
            {
                Debug.Log("Stopped spawning enemies");
                StopCoroutine(SpawnEnemies());
            }
        }

        private IEnumerator SpawnEnemies()
        {
            while (gameObject.activeSelf)
            {
                yield return new WaitForSeconds(spawnDelay);
                _bigBabyPrefabConfig.CreateGameObject(
                    _enemyTroopSpawnPoints.Next().position,
                    Quaternion.identity,
                    transform
                );
            }
        }
    }
}
