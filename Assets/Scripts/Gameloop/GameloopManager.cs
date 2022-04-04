using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;
using System;
using UnityEngine.SceneManagement;

public enum PlayerControl { MainCharacter, Camera };
public enum EntityDirection { Left, Right };


public enum GameState
{
    MainMenu,
    Gameplay,
    GameOver,
    Paused,
}

public class GameloopManager : MonoBehaviourSingleton<GameloopManager>
{
    public event Action<PlayerControl> OnSwitchPlayerControl;
    public PlayerControl _playerControl { get; private set; }


    public GameState CurrentGameState { get; private set; }

    public event Action OnMainCharacterDied;
    public event Action OnLoseGame;


    void Awake()
    {
        CurrentGameState = GameState.Gameplay;
    }



    public void SwitchPlayerControl()
    {
        if (_playerControl == PlayerControl.MainCharacter)
        {
            _playerControl = PlayerControl.Camera;
        }
        else
        {
            _playerControl = PlayerControl.MainCharacter;
        }

        OnSwitchPlayerControl?.Invoke(_playerControl);
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
