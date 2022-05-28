using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayUtils.Prefabs
{
    [System.Serializable]
    public class PrefabConfig
    {
        [SerializeField] private GameObject _prefab;

        public GameObject CreateGameObject(Vector3 position, Quaternion rotation)
        {
            return GameObject.Instantiate(_prefab, position, rotation);
        }

        public GameObject CreateGameObject(Vector3 position, Quaternion rotation, Transform parent)
        {
            return GameObject.Instantiate(_prefab, position, rotation, parent);
        }

# if UNITY_EDITOR
        public static PrefabConfig CreatePrefabConfig(GameObject prefab)
        {
            PrefabConfig config = new PrefabConfig();
            config._prefab = prefab;
            return config;
        }
#endif
    }
}