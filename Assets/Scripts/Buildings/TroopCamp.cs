using GameplayUI.Helpers;
using UnityEngine;

namespace Buildings.TroopCampComponent
{
    public class TroopCamp : BuildingCore, IDamageable
    {

        [SerializeField] private float _startingHealth = 100f;
        protected float _currentHealth;
        [SerializeField] private HealthBarUI _healthBarUI;
        [SerializeField] private TroopCampUI _troopCampUI;


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
            _healthBarUI.SetHealth(_currentHealth / _startingHealth, 0.1f);

            if (_currentHealth <= 0)
            {
                //Make troops leave building if destroyed
                DestroyBuilding();
            }
        }



        private void OnClickRepair()
        {
            _currentHealth = _startingHealth;
            _healthBarUI.SetHealth(_currentHealth / _startingHealth, 0.1f);
        }


        private void OnClickAttack(EntityDirection direction)
        {
            //Make Troops Leave Building depending on the requested directioon
        }

    }
}