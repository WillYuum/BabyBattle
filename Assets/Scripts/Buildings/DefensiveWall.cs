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


        protected override void OnAwake()
        {
            _currentHealth = _startingHealth;

            _defensiveWallUI.Init(new DefensiveWallUI.InitConfig()
            {
                StartingHealthRatio = _currentHealth / _startingHealth,
                OnClickRepairButton = OnClickRepair,
                OnClickOnAttackButton = OnClickAttack,
            });
        }



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

        private void OnClickRepair()
        {
            _currentHealth = _startingHealth;
            _defensiveWallUI.UpdateHealthBar(_currentHealth / _startingHealth);
        }


        private void OnClickAttack(EntityDirection direction)
        {
            //Make Troops Leave Building depending on the requested direction
            foreach (var troop in _troops)
            {
                troop.MoveOutOfBuilding(direction);
            }

            _troops.Clear();
        }



        protected override void OnTroopInteractWithBuilding(ITroopBuildingInteraction troop)
        {
            base.OnTroopInteractWithBuilding(troop);

            if (_troops.Count < _idleTroopsPositions.Length)
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