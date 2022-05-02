using UnityEngine;


namespace Troops.States
{
    public class TroopMoveState : TroopStateCore
    {
        public override void EnterState()
        {
            //Play running animation
        }

        public override void ExitState()
        {
        }

        public override void Execute()
        {
            _troop.Move();
        }
    }


    public class TroopIdleState : TroopStateCore
    {
        public override void EnterState()
        {
            //Play idle animation

        }

        public override void ExitState()
        {
        }

        public override void Execute()
        {
            // _troop.CheckForEenmies();
        }
    }


    public class TroopStateCore
    {
        protected Troop _troop;

        public virtual void EnterState()
        {

        }
        public virtual void Execute()
        {

        }

        public virtual void ExitState()
        {

        }

        public void ChangeState(TroopStateCore newState, Troop troop)
        {
            _troop = troop;
            ExitState();
            newState.EnterState();
        }

    }
}