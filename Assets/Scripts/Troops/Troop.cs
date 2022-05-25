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
        protected TroopType _troopType;
        protected FriendOrFoe _friendOrFoe;
        protected float CurrentHealth { get; private set; }
        protected float AttackDelay { get; private set; }
        protected float DefaultMoveSpeed { get; private set; }
        protected float CurrentMoveSpeed { get; private set; }
        protected float AttackDamage { get; private set; }
        protected Vector2 MoveDirection { get; private set; }

        protected event Action _notifyFollowers;


        private TroopState _troopState;
        private TroopStateCore _currentTroopState;


        [SerializeField] private GameObject _existenceCollider;


        public void InitTroop(TroopType troopType, EntityDirection moveDir, FriendOrFoe friendOrFoe)
        {
            TroopVariable data = GameVariables.Instance.TroopVariables.GetVariable(troopType);

            CurrentHealth = data.StartingHealth;
            AttackDelay = data.AttackDelay;
            DefaultMoveSpeed = data.MoveSpeed;
            CurrentMoveSpeed = DefaultMoveSpeed;
            AttackDamage = data.Damage;

            SetMoveDirection(moveDir);

            _friendOrFoe = friendOrFoe;

            _currentTroopState = new TroopMoveState();
            _currentTroopState.ChangeState(_currentTroopState, this);

        }

        public abstract void Attack(IDamageable targets = null, IDamageable[] multipleTargets = null);

        // public virtual void Attack(IDamageable targets, IDamageable[] multipleTargets = null)
        // {
        //     var damageAction = new TakeDamageAction
        //     {
        //         DamageAmount = AttackDamage,
        //         DamagedByTroop = _troopType,
        //     };

        //     if (multipleTargets != null)
        //     {
        //         foreach (var item in multipleTargets)
        //         {
        //             item.TakeDamage(damageAction);
        //         }
        //     }
        //     else
        //     {
        //         targets.TakeDamage(damageAction);
        //     }
        // }
    

        void Update()
        {
            _currentTroopState.Execute();
        }

        public bool CheckForEeneies(out IDamageable damageableTroops)
        {
            damageableTroops = null;


            RaycastHit2D hit = Physics2D.Raycast(transform.position, MoveDirection, 1.0f);
            if (hit.collider)
            {
                if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    damageableTroops = damageable;
                }
            }

            if (damageableTroops != null)
            {
                return true;
            }
            else
            {
                return false;
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
            Destroy(gameObject, 1.0f);
        }

        protected void SetMoveDirection(EntityDirection direction)
        {
            var position = _existenceCollider.transform.localPosition;
            if (direction == EntityDirection.Left)
            {
                position.x = -position.x;
                position.y = -position.y;
                _existenceCollider.transform.localPosition = position;
            }
            else
            {
                position.x = Math.Abs(position.x);
                position.y = Math.Abs(position.y);
                _existenceCollider.transform.localPosition = position;
            }

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

            _troopState = newState;
            _currentTroopState.ChangeState(_currentTroopState, this);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("ExistenceCollider")) { return; }

            if (collision.transform.parent.TryGetComponent<Troop>(out Troop troop))
            {
                if (troop._friendOrFoe != _friendOrFoe)
                {
                    return;
                }

                if (troop._troopState != TroopState.Moving)
                {
                    return;
                }

                bool movingInSameDirection = troop.MoveDirection == MoveDirection;
                if (movingInSameDirection)
                {
                    CurrentMoveSpeed = troop.CurrentMoveSpeed;
                }

            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("ExistenceCollider")) { return; }

            if (collision.transform.parent.TryGetComponent<Troop>(out Troop troop))
            {
                if (troop._friendOrFoe != _friendOrFoe)
                {
                    return;
                }

                if (troop._troopState != TroopState.Moving)
                {
                    return;
                }


                bool movingInSameDirection = troop.MoveDirection == MoveDirection;
                if (movingInSameDirection)
                {
                    CurrentMoveSpeed = DefaultMoveSpeed;
                }

            }
        }


        protected void NotifyFollowers()
        {
            if (_notifyFollowers != null)
            {
                _notifyFollowers.Invoke();
                _notifyFollowers = null;
            }
        }

    }
}
