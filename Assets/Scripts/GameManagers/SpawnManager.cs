using System.Collections;
using UnityEngine;
using Utils.GenericSingletons;
using GameplayUtils.Prefabs;
using Troops;
using Buildings;
namespace SpawnManagerCore
{


    /*
     SpawnManager is responsible of just spawning prefabs/objects in the world.
    */
    public class SpawnManager : MonoBehaviourSingleton<SpawnManager>
    {




        [System.Serializable]
        public class TroopToSpawn
        {
            [SerializeField] public TroopType troopType;
            [SerializeField] public PrefabConfig prefabConfig;
        }


        [Header("Troops")]
        [SerializeField] private TroopToSpawn[] _friendlyTroopsPrefabs;
        [SerializeField] private TroopToSpawn[] _enemyTroopsPrefabs;

        //TODO: Need to implement similar logic to TroopToSpawn[] when working on building logic
        [Header("buildings")]
        [SerializeField] private PrefabConfig _defensiveWallBuilding;
        [SerializeField] private PrefabConfig _troopCampBuilding;


        public Troop SpawnTroop(TroopType troopType, Vector3 position, FriendOrFoe friendOrFoe)
        {
            Troop troop = null;
            var troopArray = friendOrFoe == FriendOrFoe.Friend ? _friendlyTroopsPrefabs : _enemyTroopsPrefabs;

            for (int i = 0; i < troopArray.Length; i++)
            {
                if (troopArray[i].troopType == troopType)
                {
                    var spawnedTroop = troopArray[i].prefabConfig.CreateGameObject(position, Quaternion.identity);
                    troop = spawnedTroop.GetComponent<Troop>();
                    break;
                }
            }

            return troop;
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
