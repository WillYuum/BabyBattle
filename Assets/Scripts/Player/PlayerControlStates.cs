using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.InputsController;

namespace Player.Controls
{
    public class CameraControlState : PlayerInputState
    {
        public override void CheckInput()
        {
            base.CheckInput();
        }

    }

    public class MainCharacterState : PlayerInputState
    {
        public override void CheckInput()
        {
            base.CheckInput();


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


    public abstract class PlayerInputState
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