using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.InputsController;

namespace Player.Controls
{
    public class CameraControlState : IPlayerInput
    {
        public override void CheckInput()
        {
            base.CheckInput();
        }

    }

    public class MainCharacterState : IPlayerInput
    {
        public override void CheckInput()
        {
            base.CheckInput();

            float hor = Input.GetAxis("Horizontal");


            if (Input.GetKey(KeyCode.A))
            {
                _playerInput.HandlePlayerMove(EntityDirection.Left);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _playerInput.HandlePlayerMove(EntityDirection.Right);
            }
            else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                if (Input.GetKeyUp(KeyCode.A))
                {
                    _playerInput.HandlePlayerMove(EntityDirection.Left);
                }
                else if (Input.GetKeyUp(KeyCode.D))
                {
                    _playerInput.HandlePlayerMove(EntityDirection.Right);
                }
            }


            if (Input.GetMouseButtonDown(0))
            {
                _playerInput.HandlePlayerClickedOnMouse();
            }
        }
    }


    public class IPlayerInput
    {
        protected PlayerInput _playerInput;

        public void Init(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        public virtual void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _playerInput.ClickedOnSwitchPlayerControl(PlayerControl.Camera);
            }
        }
    }
}