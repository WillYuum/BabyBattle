using Utils.GenericSingletons;
using System;
using UnityEngine.SceneManagement;
using Troops;
using SpawnManagerCore;
using UnityEngine;
using HUDCore;
using Buildings;

public enum PlayerControl { None, MainCharacter, Camera };
public enum EntityDirection { Idle, Left, Right };


public class GameloopManager : MonoBehaviourSingleton<GameloopManager>
{
    public event Action<PlayerControl> OnSwitchPlayerControl;

    public PlayerControl PlayerControl { get; private set; }


    public int HoldingToysCount { get; private set; }
    public int MaxHoldingToysCount { get; private set; }

    public int CurrentSpawnedTroopsCount { get; private set; }
    public int MaxSpawedTroopsCount { get; private set; }


    public event Action OnMainCharacterDied;
    public event Action OnGameLoopStarted;
    public event Action OnLoseGame;


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

        HUD.instance.OnUpdateTroopsSpawnCount.Invoke();
        HUD.instance.OnUpdateToysCount.Invoke();

        OnGameLoopStarted?.Invoke();
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


    public void MainCharacterDied()
    {
        OnMainCharacterDied?.Invoke();
        OnMainCharacterDied = null;

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

        var troop = SpawnManager.instance.SpawnFriendlyTroop(spawnTroopAction.TroopType, spawnTroopAction.SpawnPoint);
        troop.InitTroop(spawnTroopAction.MoveDirection);

        HUD.instance.OnUpdateTroopsSpawnCount.Invoke();
        HUD.instance.OnUpdateToysCount.Invoke();
    }


    public void CollectToys(CollectToysEvent collectToysEvent)
    {
        if (HoldingToysCount + collectToysEvent.ToysCount > MaxHoldingToysCount)
        {
            //Display can't collect toys message
        }
        else
        {
            if (collectToysEvent.CollectedToy)
            {
                Destroy(collectToysEvent.CollectedToy);
            }

            HoldingToysCount += collectToysEvent.ToysCount;
            HUD.instance.OnUpdateToysCount.Invoke();
        }
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

}

interface IDamageable
{
    void TakeDamage(TakeDamageAction damage);
}


public class ConstructBuildingAction
{
    public BuildingType BuildingType;
    public int Cost;
    public Vector3 SpawnPoint;
    // public FriendOrFoe FriendOrFoe;
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
    public TakeDamageAction(int damageAmount, TroopType damagedByTroop)
    {
        DamageAmount = damageAmount;
        DamagedByTroop = damagedByTroop;
    }
}