using UnityEngine;
using Buildings;

public class MainCamp : BuildingCore, IDamageable
{
    private EntityDirection _spawnDirection = EntityDirection.Left;

    [Header("References")]
    [SerializeField] private MainCampUI _campUi;

    protected override void OnAwake()
    {
        base.OnAwake();

        _campUi.InitUI(new MainCampUI.InitConfig
        {
            active = true,
            startingHealth = _hp.GetHPPercentage(),
            onClickTroopCard = OnClickTroopCard
        });
    }

    public void TakeDamage(TakeDamageAction damageAction)
    {
        _hp.TakeDamage(damageAction.DamageAmount);
        _campUi.UpdateHealthBar(_hp.GetHPPercentage());

        if (_hp.CurrentHP <= 0)
        {
            Debug.Log("Building destroyed");
            GameloopManager.instance.MainBuildingDestroyed();
        }
    }

    private void OnClickTroopCard(Troops.TroopType troopType)
    {
        GameloopManager.instance.InvokeSpawnFriendlyTroop(new SpawnTroopAction
        {
            MoveDirection = _spawnDirection,
            TroopType = troopType,
            SpawnPoint = transform.position,
            TroopCost = 3,
        });
    }

    public void SwitchSpawnDirection()
    {
        if (_spawnDirection == EntityDirection.Left)
        {
            _spawnDirection = EntityDirection.Right;
        }
        else
        {
            _spawnDirection = EntityDirection.Left;
        }

        RenderSwitchDirection();
    }


    private void RenderSwitchDirection()
    {

    }
}
