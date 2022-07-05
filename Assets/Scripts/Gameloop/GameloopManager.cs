using Utils.GenericSingletons;
using System;
using UnityEngine.SceneManagement;
using Troops;
using SpawnManagerCore;
using UnityEngine;
using HUDCore;
using Player.InputsController;
using Buildings;
using Territory;
using System.Collections;
using Utils.ArrayUtils;

public enum PlayerControl { None, MainCharacter, Camera };
public enum EntityDirection { Idle, Left, Right };

public enum FriendOrFoe { Friend, Foe };
public class GameloopManager : MonoBehaviourSingleton<GameloopManager>
{
    public event Action<PlayerControl> OnSwitchPlayerControl;

    [SerializeField] public PlayerControl PlayerControl { get; private set; }

    [SerializeField] public ToyGeneratorCore ToyGenerator;


    [SerializeField] private PlayerInputActions _playerInputActions;

    public int HoldingToysCount { get; private set; }
    public int MaxHoldingToysCount { get; private set; }

    public int CurrentSpawnedTroopsCount { get; private set; }
    public int MaxSpawedTroopsCount { get; private set; }



    public event Action OnGameLoopStarted;
    public event Action OnLoseGame;

    [SerializeField] private PseudoRandArray<Transform> _enemyTroopSpawnPoints;



    public Territory.TerritoryCore HoveredTerritory { get; private set; }


    void Start()
    {
        StartGameLoop();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CollectToys(new CollectToysEvent
            {
                ToysCount = 1
            });
        }
    }

    public void StartGameLoop()
    {
        PlayerControl = PlayerControl.MainCharacter;
        OnSwitchPlayerControl.Invoke(PlayerControl);

        //NOTE: This is a temporary so you can start with something when game starts 
        HoldingToysCount = 5;
        MaxHoldingToysCount = 15;

        CurrentSpawnedTroopsCount = 0;
        MaxSpawedTroopsCount = 10;

        Invoke(nameof(StartSpawningEnemies), 1f);

        HUD.instance.OnUpdateTroopsSpawnCount.Invoke();
        HUD.instance.OnUpdateToysCount.Invoke();

        OnGameLoopStarted?.Invoke();
    }


    [SerializeField] private BigEnemyCamp _bigEnemyCamp;
    private void StartSpawningEnemies()
    {
        //TODO: Spawn enemies within intervals
        // StartCoroutine(nameof(SpawnEnemiesRandomly));

        //TODO: Spawn enemies and make them enter the bigEnemyCamp for the big attack
        StartCoroutine(AddEnemyTroopsToBigCamp(_bigEnemyCamp, 5, 1.0f));
    }

    private IEnumerator SpawnEnemiesRandomly()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(3.0f);
            var troop = SpawnManager.instance.SpawnTroop(TroopType.SharpShooter, _enemyTroopSpawnPoints.PickNext().position, FriendOrFoe.Foe);
            troop.InitTroop(EntityDirection.Right);
        }
    }

    private IEnumerator AddEnemyTroopsToBigCamp(BigEnemyCamp enemyCamp, int troopsCount, float interval)
    {
        int troopsAdded = 0;
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(interval);

            Vector3 spawnPos = enemyCamp.transform.position;

            var troop = SpawnManager.instance.SpawnTroop(TroopType.SharpShooter, spawnPos, FriendOrFoe.Foe);
            troop.InitTroop(enemyCamp.AttackDirection);

            enemyCamp.GarrisonTroops(troop);
            troop.GetComponent<ITroopBuildingInteraction>().MoveToIdlePositionInBuilding(enemyCamp.transform);


            troopsAdded++;
            if (troopsAdded >= troopsCount)
            {
                StopCoroutine(nameof(AddEnemyTroopsToBigCamp));
                SendWaveOfEnemiesToAttack(enemyCamp);
                break;
            }
        }
    }

    private void SendWaveOfEnemiesToAttack(BigEnemyCamp camp)
    {
        camp.UnGarrisonTroops();
    }


    public void SwitchPlayerControl()
    {
        if (PlayerControl == PlayerControl.MainCharacter)
        {
            PlayerControl = PlayerControl.Camera;
        }
        else
        {
            PlayerControl = PlayerControl.MainCharacter;
        }


        OnSwitchPlayerControl?.Invoke(PlayerControl);
    }

    public void MainBuildingDestroyed()
    {
        OnLoseGame?.Invoke();
        OnLoseGame = null;
    }



    public void InvokeSpawnFriendlyTroop(SpawnTroopAction spawnTroopAction)
    {
        bool canSpawnTroop = CurrentSpawnedTroopsCount < MaxSpawedTroopsCount;
        bool sufficientToys = HoldingToysCount > spawnTroopAction.TroopCost;
        if (!canSpawnTroop || !sufficientToys)
        {
            return;
        }



        HoldingToysCount -= spawnTroopAction.TroopCost;
        CurrentSpawnedTroopsCount++;

        var troop = SpawnManager.instance.SpawnTroop(spawnTroopAction.TroopType, spawnTroopAction.SpawnPoint, FriendOrFoe.Friend);
        troop.InitTroop(spawnTroopAction.MoveDirection);

        HUD.instance.OnUpdateTroopsSpawnCount.Invoke();
        HUD.instance.OnUpdateToysCount.Invoke();
    }


    public void CollectToys(CollectToysEvent collectToysEvent)
    {
        if (MaxHoldingToysCount == HoldingToysCount)
        {
            return;
        }

        if (collectToysEvent.CollectedToy)
        {
            Destroy(collectToysEvent.CollectedToy);
        }

        if (HoldingToysCount + collectToysEvent.ToysCount > MaxHoldingToysCount)
        {
            HoldingToysCount = MaxHoldingToysCount;
        }
        else
        {

            HoldingToysCount += collectToysEvent.ToysCount;
        }

        HUD.instance.OnUpdateToysCount.Invoke();
    }


    public void HandleConstructBuilding(ConstructBuildingAction constructBuildingAction, Action onStartBuilding)
    {
        if (HoldingToysCount < constructBuildingAction.Cost)
        {
            //Display can't construct building message
        }
        else
        {
            HoldingToysCount -= constructBuildingAction.Cost;
            HUD.instance.OnUpdateToysCount.Invoke();

            SpawnManager.instance.SpawnBuilding(constructBuildingAction.BuildingType, constructBuildingAction.SpawnPoint);

            onStartBuilding.Invoke();
        }
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void TryTakeOverTerritory(ControlledBy controlledBy)
    {
        if (HoveredTerritory == null)
        {
            Debug.LogWarning("No territory hovered");
            return;
        }

        HoveredTerritory.TryTakeControl(controlledBy);

    }

    public void IncreaseMaxAmountOfTroops(int amount)
    {
        MaxSpawedTroopsCount += amount;
        HUD.instance.OnUpdateTroopsSpawnCount.Invoke();
    }

    public void DecreaseMaxAmountOfTroops(int amount)
    {
        MaxSpawedTroopsCount -= amount;
        HUD.instance.OnUpdateTroopsSpawnCount.Invoke();
    }

}

public interface IDamageable
{
    void TakeDamage(TakeDamageAction damage);
}


public class ConstructBuildingAction
{
    public BuildingType BuildingType;
    public int Cost;
    public Vector3 SpawnPoint;
}

public struct CollectToysEvent
{
    public int ToysCount;
    public GameObject CollectedToy;
}


public struct SpawnTroopAction
{
    public TroopType TroopType;
    public EntityDirection MoveDirection;
    public int TroopCost;
    public Vector3 SpawnPoint;
}

public struct TakeDamageAction
{
    public float DamageAmount;
    public TroopType DamagedByTroop;
}