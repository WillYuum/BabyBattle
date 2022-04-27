using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;
using Utils.ArrayUtils;
using GameplayUtils.Prefabs;
using Troops;

namespace SpawnManagerCore
{
    [System.Serializable]
    public struct TroopPrefabConfig
    {
        public TroopType troopType;
        public PrefabConfig prefabConfig;
    }
    public class SpawnManager : MonoBehaviourSingleton<SpawnManager>
    {

        [SerializeField] private TroopPrefabConfig[] _friendlyTroopsPrefabs;


        [SerializeField] private PseudoRandArray<Transform> _enemyTroopSpawnPoints;


        [SerializeField] private PrefabConfig _bigBabyPrefabConfig;

        [SerializeField] private float spawnDelay = 3.0f;


        void Awake()
        {
            // GameloopManager.instance.OnGameLoopStarted += () => ToggleSpawnEnemy(true);
            // GameloopManager.instance.OnLoseGame += () => ToggleSpawnEnemy(false);
        }

        public Troop SpawnFriendlyTroop(TroopType troopType)
        {
            for (int i = 0; i < _friendlyTroopsPrefabs.Length; i++)
            {
                if (_friendlyTroopsPrefabs[i].troopType == troopType)
                {
                    var mousePositionToWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    return _friendlyTroopsPrefabs[i].prefabConfig.CreateGameObject(mousePositionToWorldPosition, Quaternion.identity).GetComponent<Troop>();
                }
            }

#if UNITY_EDITOR
            Debug.LogError("No prefab found for troop type: " + troopType);
#endif

            return null;
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
