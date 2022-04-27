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