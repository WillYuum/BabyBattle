using UnityEngine;
using Player.InputsController;

namespace Player.Controls
{
    public class PlayerIdleState : PlayerInputStateCore
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

    public class MainCharacterIdleState : PlayerInputStateCore
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


    public class InTerritoryAreaInputs : PlayerInputStateCore
    {
        public override void CheckInput()
        {
            base.CheckInput();

            if (Input.GetKey(KeyCode.E))
            {
                // _playerActions.EnterIdleStateWhileTryingToSpawnTroop();
                GameloopManager.instance.TryTakeOverTerritory(Territory.ControlledBy.Friend);
            }


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