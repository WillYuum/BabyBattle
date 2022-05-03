using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;
using Utils.ArrayUtils;
using GameplayUtils.Prefabs;
using Troops;
using Buildings;
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

        [SerializeField] private PrefabConfig _troopCampBuilding;
        [SerializeField] private PrefabConfig _defensiveWallBuilding;


        [SerializeField] private PseudoRandArray<Transform> _enemyTroopSpawnPoints;


        [SerializeField] private PrefabConfig _bigBabyPrefabConfig;

        [SerializeField] private float spawnDelay = 3.0f;


        void Awake()
        {
            // GameloopManager.instance.OnGameLoopStarted += () => ToggleSpawnEnemy(true);
            // GameloopManager.instance.OnLoseGame += () => ToggleSpawnEnemy(false);
        }

        public Troop SpawnFriendlyTroop(TroopType troopType, Vector3 position)
        {
            for (int i = 0; i < _friendlyTroopsPrefabs.Length; i++)
            {
                if (_friendlyTroopsPrefabs[i].troopType == troopType)
                {
                    return _friendlyTroopsPrefabs[i].prefabConfig.CreateGameObject(position, Quaternion.identity).GetComponent<Troop>();
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
                    _enemyTroopSpawnPoints.PickNext().position,
                    Quaternion.identity,
                    transform
                );
            }
        }



        public GameObject SpawnBuilding(BuildingType buildingType, Vector3 position)
        {
            switch (buildingType)
            {
                case BuildingType.TroopCamp:
                    return _troopCampBuilding.CreateGameObject(position, Quaternion.identity);
                case BuildingType.DefensiveWall:
                    return _defensiveWallBuilding.CreateGameObject(position, Quaternion.identity);
                default:
                    Debug.LogError("No prefab found for building type: " + buildingType);
                    return _troopCampBuilding.CreateGameObject(position, Quaternion.identity);
            }
        }
    }
}
