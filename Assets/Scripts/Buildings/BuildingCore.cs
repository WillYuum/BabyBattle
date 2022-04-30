using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Troops;
using GameplayUI.Helpers;


namespace Buildings
{
    public enum FriendOrFoe { Friend, Foe };
    public enum BuildingType { Destroyed, TroopCamp, DefensiveWall };

    public class BuildingCore : MonoBehaviour, IDamageable
    {
        [SerializeField] protected FriendOrFoe _friendOrFoe;
        [SerializeField] protected BuildingType _buildingType;
        [SerializeField] private float _startingHealth = 100f;
        protected float _currentHealth;


        [SerializeField] private HealthBarUI _healthBarUI;

        // private List<Troop> _troops = new List<Troop>();


        void Awake()
        {
            _currentHealth = _startingHealth;
            _healthBarUI.Init();
        }

        public void TakeDamage(TakeDamageAction damageAction)
        {
            _currentHealth -= damageAction.DamageAmount;
            _healthBarUI.SetHealth(_currentHealth / _startingHealth, 0.1f);

            if (_currentHealth <= 0)
            {
                // MakeTroopsLeaveBuilding(damageAction);
                DestroyBuilding();
            }
        }

        public virtual void InteractWithBuilding()
        {

        }

        public virtual void DestroyBuilding()
        {
            Destroy(gameObject, 2.0f);
        }
    }
}