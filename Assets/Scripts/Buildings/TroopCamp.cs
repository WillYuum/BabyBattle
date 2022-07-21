using System.Collections.Generic;
using GameplayUI.Helpers;
using Troops;
using UnityEngine;
using Utils.ArrayUtils;

namespace Buildings.TroopCampComponent
{
    public class TroopCamp : BuildingCore, IDamageable
    {

        [SerializeField] private float _startingHealth = 100f;
        protected float _currentHealth;
        [SerializeField] private TroopCampUI _troopCampUI;

        private List<ITroopBuildingInteraction> _troops = new List<ITroopBuildingInteraction>();

        [SerializeField] private PseudoRandArray<Transform> _idlePositions;



        protected override void OnAwake()
        {
            _currentHealth = _startingHealth;

            _troopCampUI.Init(new TroopCampUI.InitConfig()
            {
                StartingHealthRatio = _currentHealth / _startingHealth,
                OnClickRepairButton = OnClickRepair,
                OnClickOnArrowButton = OnClickAttack,
            });
        }


        public void TakeDamage(TakeDamageAction damageAction)
        {
            _currentHealth -= damageAction.DamageAmount;
            _troopCampUI.UpdateHealthBar(_currentHealth / _startingHealth);

            if (_currentHealth <= 0)
            {
                //Make troops leave building if destroyed
                DestroyBuilding();
            }
        }



        private void OnClickRepair()
        {
            _currentHealth = _startingHealth;
            _troopCampUI.UpdateHealthBar(_currentHealth / _startingHealth);
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



        // protected override void OnTroopInteractWithBuilding(ITroopBuildingInteraction troop)
        // {
        //     base.OnTroopInteractWithBuilding(troop);

        //     if (_troops.Count < _idlePositions.Length)
        //     {
        //         _troops.Add(troop);
        //         InvokeTroopIdleInBuilding(troop);
        //     }
        // }

        private void InvokeTroopIdleInBuilding(ITroopBuildingInteraction troop)
        {

            troop.MoveToIdlePositionInBuilding(_idlePositions.PickNext().position);
        }

    }
}
