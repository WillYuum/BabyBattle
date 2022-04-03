using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;

public class PFXManager : MonoBehaviourSingleton<PFXManager>
{
    [SerializeField] public PFXConfig[] _pfxConfigs;

    public void PlayPfx(string pfxName, Vector3 position)
    {
        PFXConfig pfxConfig = Array.Find(_pfxConfigs, item => item.pfxName == pfxName);

#if UNITY_EDITOR
        if (pfxConfig == null)
        {
            Debug.LogError("PFXManager: PFXConfig not found for pfxName: " + pfxName);
        }
#endif

        CreatePFX(pfxConfig.pfxPrefab, position).Play();
    }

    private ParticleSystem CreatePFX(GameObject pfxPrefab, Vector3 position)
    {
        return Instantiate(pfxPrefab, position, Quaternion.identity, transform).GetComponent<ParticleSystem>();
    }

}


[System.Serializable]
public class PFXConfig
{
    [SerializeField] public string pfxName;
    [SerializeField] public GameObject pfxPrefab;
}