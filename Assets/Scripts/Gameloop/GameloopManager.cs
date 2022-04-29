using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;
using System;
using UnityEngine.SceneManagement;
using HUDCore;
using Troops;
using SpawnManagerCore;
using UnityEngine;

public enum PlayerControl { None, MainCharacter, Camera };
public enum EntityDirection { Idle, Left, Right };


public class GameloopManager : MonoBehaviourSingleton<GameloopManager>
{
    public event Action<PlayerControl> OnSwitchPlayerControl;

    public PlayerControl PlayerControl { get; private set; }




    public event Action OnMainCharacterDied;
    public event Action OnGameLoopStarted;
    public event Action OnLoseGame;


    void Start()
    {
        StartGameLoop();
    }

    public void StartGameLoop()
    {
        PlayerControl = PlayerControl.MainCharacter;
        OnSwitchPlayerControl.Invoke(PlayerControl);


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
        if (spawnTroopAction.TroopCost + ToysUsedCount > ToysMaxUseCount)
        {
            //TODO: Show message that you can't spawn troop
            return;
        }

        var troop = SpawnManager.instance.SpawnFriendlyTroop(spawnTroopAction.TroopType, spawnTroopAction.SpawnPoint);
        troop.InitTroop(spawnTroopAction.MoveDirection);

        //Should update UI for toys
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