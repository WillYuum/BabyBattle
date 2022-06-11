using UnityEngine;


namespace Troops.States
{

    public class TroopAttackState : TroopStateCore
    {
        private float _delayTillHit;
        public override void EnterState()
        {
            _delayTillHit = _troop.AttackDelay;
            _troop.Animator.Play("Attack");
        }

        public override void ExitState()
        {
        }

        public override void Execute()
        {
            _delayTillHit -= Time.deltaTime;
            if (_delayTillHit <= 0)
            {
                _troop.Attack();
                _delayTillHit = _troop.AttackDelay;
            }
        }
    }

    public class TroopMoveState : TroopStateCore
    {
        public override void EnterState()
        {
            //Play running animation
            _troop.Animator.Play("Walk");
        }

        public override void ExitState()
        {
        }

        public override void Execute()
        {
            _troop.Move();
            _troop.FindTargetToAttack();
        }
    }


    public class TroopIdleState : TroopStateCore
    {
        public override void EnterState()
        {
            //Play idle animation
            _troop.Animator.Play("Idle");
        }

        public override void ExitState()
        {
        }

        public override void Execute()
        {
            _troop.FindTargetToAttack();
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