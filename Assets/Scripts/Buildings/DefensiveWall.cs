using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.ArrayUtils;
using Troops;

namespace Buildings.DefensiveWallComponent
{
    public class DefensiveWall : BuildingCore, IDamageable
    {

        [SerializeField] private DefensiveWallUI _defensiveWallUI;
        [SerializeField] private float _startingHealth = 100f;
        protected float _currentHealth;
        [SerializeField] private PseudoRandArray<Transform> _idleTroopsPositions;

        private List<ITroopBuildingInteraction> _troops = new List<ITroopBuildingInteraction>();

        public void TakeDamage(TakeDamageAction damageAction)
        {
            _currentHealth -= damageAction.DamageAmount;
            _defensiveWallUI.UpdateHealthBar(_currentHealth / _startingHealth);

            if (_currentHealth <= 0)
            {
                //Make troops leave building if destroyed
                DestroyBuilding();
            }
        }


        protected override void OnTroopInteractWithBuilding(ITroopBuildingInteraction troop)
        {
            base.OnTroopInteractWithBuilding(troop);

            if (troop.TryAccessBuilding(this) && _troops.Count < _idleTroopsPositions.Length)
            {
                _troops.Add(troop);
                InvokeTroopIdleInBuilding(troop);
            }
        }


        private void InvokeTroopIdleInBuilding(ITroopBuildingInteraction troop)
        {

            troop.MoveToIdlePositionInBuilding(_idleTroopsPositions.PickNext());
        }

    }
}