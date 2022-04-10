using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;
using Player.Controls;


namespace Player.InputsController
{
    public class PlayerInput : MonoBehaviour
    {
        private CameraControlState _cameraControlState = new CameraControlState();
        private MainCharacterState _mainCharacterState = new MainCharacterState();

        private PlayerInputState _currentPlayerInput;


        void Awake()
        {
            _cameraControlState.Init(this);
            _mainCharacterState.Init(this);

            GameloopManager.instance.OnSwitchPlayerControl += OnPlayerControlSwitch;
        }

        void Update()
        {
            _currentPlayerInput.CheckInput();
        }


        private void OnPlayerControlSwitch(PlayerControl playerControl)
        {
            if (playerControl == PlayerControl.MainCharacter)
            {
                _currentPlayerInput = _mainCharacterState;
            }
            else
            {
                _currentPlayerInput = _cameraControlState;
            }
        }


        public void HandlePlayerMove(EntityDirection direction)
        {
            MainCharacter.instance.Move(direction);
        }


        public void HandlePlayerClickedOnMouse()
        {
            MainCharacter.instance.Attack();
        }


        public void ClickedOnSwitchPlayerControl(PlayerControl playerControl)
        {
            GameloopManager.instance.SwitchPlayerControl();
        }

    }
}