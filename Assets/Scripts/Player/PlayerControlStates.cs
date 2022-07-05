using UnityEngine;
using Player.InputsController;

namespace Player.Controls
{

    public class NormalPlayerInput : PlayerInputStateCore
    {
        public override void CheckInput()
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                _playerActions.InvokeMoveCamera(EntityDirection.Left);
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                _playerActions.InvokeMoveCamera(EntityDirection.Right);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _playerActions.InvokeClickScreen();
            }
        }
    }


    public class LockedPlayerInput : PlayerInputStateCore
    {
        public override void CheckInput()
        {

        }
    }


    public abstract class PlayerInputStateCore
    {
        protected PlayerInputActions _playerActions;

        public void Init(PlayerInputActions playerInput)
        {
            _playerActions = playerInput;
        }

        public virtual void CheckInput()
        {

        }
    }
}