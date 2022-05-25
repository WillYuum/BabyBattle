using UnityEngine;


namespace Troops.States
{

    public class TroopAttackState : TroopStateCore
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
            if (_troop.CheckForEeneies(out IDamageable damageable))
            {
                _troop.Attack(damageable);
            }

        }
    }

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


    public abstract class TroopStateCore
    {
        protected Troop _troop;

        public abstract void EnterState();
        public abstract void Execute();
        public abstract void ExitState();

        public void ChangeState(TroopStateCore newState, Troop troop)
        {
            _troop = troop;
            ExitState();
            newState.EnterState();
        }

    }
}