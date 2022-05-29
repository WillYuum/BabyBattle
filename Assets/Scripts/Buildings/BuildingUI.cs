using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Buildings
{

    public class BuildingUI : MonoBehaviour
    {
        [SerializeField] protected GameObject[] _objectToToggle;
        public void ToggleBuildingUI(bool active)
        {
#if UNITY_EDITOR
            if (active == gameObject.activeSelf)
            {
                Debug.LogWarning("Trying to toggle building UI when it's already in the desired state");
            }
#endif

            foreach (GameObject go in _objectToToggle)
            {
                go.SetActive(active);
            }
        }



    }
}