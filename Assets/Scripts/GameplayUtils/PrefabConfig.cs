using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayUtils.Prefabs
{
    [System.Serializable]
    public class PrefabConfig
    {
        [SerializeField] private GameObject prefab;

        public GameObject CreateGameObject(Vector3 position, Quaternion rotation)
        {
            return GameObject.Instantiate(prefab, position, rotation);
        }

        public GameObject CreateGameObject(Vector3 position, Quaternion rotation, Transform parent)
        {
            return GameObject.Instantiate(prefab, position, rotation, parent);
        }
    }
}