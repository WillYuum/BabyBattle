using System.Collections;
using System.Collections.Generic;
using GameplayUI.Helpers;
using UnityEngine;

public class MainCamp : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _currentHealth = 100f;

    [Header("References")]
    [SerializeField] private HealthBarUI _healthBarUI;

    private void Awake()
    {
        _healthBarUI.Init();
        _healthBarUI.SetHealth(_currentHealth / 100f);
    }


    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _healthBarUI.SetHealth(_currentHealth / 100f);

        if (_currentHealth <= 0)
        {
            Debug.Log("Building destroyed");
            GameloopManager.instance.MainBuildingDestroyed();
        }
    }

}
