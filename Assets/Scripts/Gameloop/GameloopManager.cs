using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;
using System;
using UnityEngine.SceneManagement;
using HUDCore;

public enum PlayerControl { None, MainCharacter, Camera };
public enum EntityDirection { Idle, Left, Right };


public class GameloopManager : MonoBehaviourSingleton<GameloopManager>
{
    public event Action<PlayerControl> OnSwitchPlayerControl;

    public PlayerControl PlayerControl { get; private set; }




    public event Action OnMainCharacterDied;
    public event Action OnLoseGame;


    void Start()
    {
        StartGameLoop();
    }

    public void StartGameLoop()
    {
        PlayerControl = PlayerControl.MainCharacter;
        OnSwitchPlayerControl.Invoke(PlayerControl);
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


    public void MainCharacterDied()
    {
        OnMainCharacterDied?.Invoke();
        OnMainCharacterDied = null;

        OnLoseGame?.Invoke();
        OnLoseGame = null;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}