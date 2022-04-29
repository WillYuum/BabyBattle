using UnityEngine;
using Player.InputsController;

namespace Player.Controls
{
    public class PlayerIdleState : PlayerInputState
    {
        public override void CheckInput()
        {
            base.CheckInput();

            if (Input.GetMouseButtonDown(0))
            {
                _playerActions.OnClickInIdle();
            }
        }
    }

    public class MainCharacterIdleState : PlayerInputState
    {
        public override void CheckInput()
        {
            base.CheckInput();


            if (Input.GetKey(KeyCode.A))
            {
                _playerActions.HandlePlayerMove(EntityDirection.Left);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _playerActions.HandlePlayerMove(EntityDirection.Right);
            }
        }
    }



    public abstract class PlayerInputState
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