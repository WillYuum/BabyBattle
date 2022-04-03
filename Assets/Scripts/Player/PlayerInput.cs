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

        private IPlayerInput _currentPlayerInput;


        void Awake()
        {
            _cameraControlState.Init(this);
            _mainCharacterState.Init(this);

            _currentPlayerInput = _mainCharacterState;
        }

        void Update()
        {
            _currentPlayerInput.CheckInput();
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