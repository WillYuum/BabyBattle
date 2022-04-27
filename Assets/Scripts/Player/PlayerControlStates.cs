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

    public class TryingToSpawnCharacter : PlayerInputState
    {
        public override void CheckInput()
        {
            base.CheckInput();

            float swipeLength = (Input.mousePosition - _playerActions.StartingMousePos).magnitude;
            if (swipeLength > _playerActions.MinDistanceToSpawnTroop)
            {
                if (Input.GetMouseButton(0))
                {
                    _playerActions.EnterCanSpawnTroopState();
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    _playerActions.EnterIdleStateWhileTryingToSpawnTroop();
                }
            }

            if (Input.GetMouseButton(0))
            {
                _playerActions.OnSwipeWithSpawnCharacter();
            }
        }
    }


    public class CanSpawnTroopState : PlayerInputState
    {
        public override void CheckInput()
        {
            base.CheckInput();

            float swipeLength = (Input.mousePosition - _playerActions.StartingMousePos).magnitude;
            if (swipeLength < _playerActions.MinDistanceToSpawnTroop)
            {
                _playerActions.EnterTryingToSpawnTroopState();
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _playerActions.InvokeSpawnTroop();
            }

            if (Input.GetMouseButton(0))
            {
                _playerActions.OnSwipeWithSpawnCharacter();
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