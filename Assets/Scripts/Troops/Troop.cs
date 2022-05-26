using System.Collections;
using System.Collections.Generic;
using Buildings;
using DG.Tweening;
using UnityEngine;
namespace Troops
{
    using System;
    using States;

    public enum TroopType { BabyTroop, LargeBaby, MortarBaby };
    public enum TroopState { Idle, Moving, Attacking };

    public interface ITroopBuildingInteraction
    {
        bool TryAccessBuilding(BuildingCore buildingCore);
        void MoveToIdlePositionInBuilding(Transform target);
        void MoveOutOfBuilding(EntityDirection direction);
    }



    public abstract class Troop : MonoBehaviour, IDamageable
    {
        public TroopType TroopType { get; private set; }
        public FriendOrFoe FriendOrFoe { get; private set; }
        public float CurrentHealth { get; private set; }
        public float AttackDelay { get; private set; }
        public float DefaultMoveSpeed { get; private set; }
        public float CurrentMoveSpeed { get; private set; }
        public float AttackDamage { get; private set; }
        public Vector2 MoveDirection { get; private set; }
        public float attackDistance { get; private set; }

        //TODO: it's better the current to know about only one troop behind him
        protected event Action _notifyFollowers;


        public TroopState TroopState { get; private set; }
        private TroopStateCore _currentTroopState;



        [SerializeField] private GameObject _characterVisual;

        void Update()
        {
            _currentTroopState.Execute();
        }


        public void InitTroop(TroopType troopType, EntityDirection moveDir, FriendOrFoe friendOrFoe)
        {
            TroopVariable data = GameVariables.Instance.TroopVariables.GetVariable(troopType);

            CurrentHealth = data.StartingHealth;
            AttackDelay = data.AttackDelay;
            DefaultMoveSpeed = data.MoveSpeed;
            CurrentMoveSpeed = DefaultMoveSpeed;
            AttackDamage = data.Damage;

            attackDistance = 1f;

            SetMoveDirection(moveDir);

            FriendOrFoe = friendOrFoe;

            _currentTroopState = new TroopMoveState();
            _currentTroopState.ChangeState(_currentTroopState, this);
        }


        public abstract void Attack();
        public void FindEnemiesToAttack()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, MoveDirection, attackDistance);
            Collider2D collider = hit.collider;
            if (collider)
            {
                if (collider.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    if (collider.GetComponent<Troop>().FriendOrFoe != FriendOrFoe)
                    {
                        ChangeState(TroopState.Attacking);
                    }
                }
            }
        }


        public void Move()
        {
            transform.position += (Vector3)MoveDirection * CurrentMoveSpeed * Time.deltaTime;
        }

        public virtual void TakeDamage(TakeDamageAction damageAction)
        {

            CurrentHealth -= damageAction.DamageAmount;
            if (CurrentHealth <= 0)
            {
                NotifyFollowers();
                Die();
            }
        }


        public virtual void Die()
        {
            CancelInvoke();
            Destroy(gameObject, 1.0f);
        }

        protected void SetMoveDirection(EntityDirection direction)
        {
            _characterVisual.transform.localScale = new Vector3(direction == EntityDirection.Left ? -1.0f : 1.0f, 1.0f, 1.0f);
            MoveDirection = direction == EntityDirection.Left ? Vector2.left : Vector2.right;
        }



        protected void ChangeState(TroopState newState)
        {
            switch (newState)
            {
                case TroopState.Idle:
                    _currentTroopState = new TroopIdleState();
                    break;
                case TroopState.Moving:
                    _currentTroopState = new TroopMoveState();
                    break;
                case TroopState.Attacking:
                    _currentTroopState = new TroopAttackState();
                    break;
                default:
                    _currentTroopState = new TroopIdleState();
                    Debug.Log("Invalid Troop State");
                    break;
            }

            TroopState = newState;
            _currentTroopState.ChangeState(_currentTroopState, this);
        }

        public void SetCurrentMoveSpeed(float speed)
        {
            CurrentMoveSpeed = speed;
        }


        protected void NotifyFollowers()
        {
            if (_notifyFollowers != null)
            {
                _notifyFollowers.Invoke();
                _notifyFollowers = null;
            }
        }

        public IDamageable[] _targets { get; private set; }
        protected void LockOnTarget(IDamageable[] target)
        {
            _targets = target;
        }

    }
}
