using UnityEngine;

public class MainCamp : MonoBehaviour, IDamageable
{
    [Header("Variables")]
    [SerializeField] private float _currentHealth = 100f;
    private EntityDirection _spawnDirection = EntityDirection.Left;

    [Header("References")]
    [SerializeField] private MainCampUI _mainCampUI;

    private void Awake()
    {
        _mainCampUI.InitUI(new MainCampUI.InitConfig
        {
            active = true,
            startingHealth = _currentHealth / 100f,
            onClickTroopCard = OnClickTroopCard
        });
    }


    public void TakeDamage(TakeDamageAction damageAction)
    {
        _currentHealth -= damageAction.DamageAmount;
        _mainCampUI.UpdateHealthBar(_currentHealth / 100f);

        if (_currentHealth <= 0)
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
