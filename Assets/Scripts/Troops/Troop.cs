using System.Collections;
using System.Collections.Generic;
using Buildings;
using DG.Tweening;
using UnityEngine;
namespace Troops
{
    using States;

    public enum TroopType { BabyTroop, LargeBaby, MortarBaby };
    public enum TroopState { Idle, Moving, Attacking };


    public interface ITroopBuildingInteraction
    {
        bool TryAccessBuilding(BuildingCore buildingCore);
        void MoveToIdlePositionInBuilding(Transform target);
        void MoveOutOfBuilding(EntityDirection direction);
    }



    public class Troop : MonoBehaviour, IDamageable, ITroopBuildingInteraction
    {
        protected TroopType _troopType;
        protected float _currentHealth { get; private set; }
        protected float _attackDelay { get; private set; }
        protected float _moveSpeed { get; private set; }
        protected float _attackDamage { get; private set; }
        protected Vector2 _moveDirection { get; private set; }

        private TroopStateCore _currentTroopState;


        public void InitTroop(TroopType troopType, EntityDirection moveDir)
        {
            // _currentHealth = GameVariables.Instance.
            TroopVariable data = GameVariables.Instance.TroopVariables.GetVariable(troopType);

            _currentHealth = data.StartingHealth;
            _attackDelay = data.AttackDelay;
            _moveSpeed = data.MoveSpeed;
            _attackDamage = data.Damage;

            _moveDirection = moveDir == EntityDirection.Left ? Vector2.left : Vector2.right;

            _currentTroopState = new TroopMoveState();
            _currentTroopState.ChangeState(_currentTroopState, this);

        }

        public virtual void Attack()
        {
            var damageAction = new TakeDamageAction
            {
                DamageAmount = _attackDamage,
                DamagedByTroop = _troopType,
            };


            //Raycast towards direction and check if target is hit

            //If target is hit, get the target's IDamageable interface and call TakeDamage() and pass the  damage action
        }

        void Update()
        {
            _currentTroopState.Execute();
        }

        public void CheckForEenmies()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _moveDirection, 1.0f);
            if (hit.collider)
            {
                if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    // damageable.TakeDamage(new TakeDamageAction(10, _troopType));
                }
            }
        }

        public void Move()
        {
            transform.position += (Vector3)_moveDirection * _moveSpeed * Time.deltaTime;
        }

        public virtual void TakeDamage(TakeDamageAction damageAction)
        {
            _currentHealth -= damageAction.DamageAmount;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }


        public virtual void Die()
        {
            Destroy(gameObject, 1.0f);
        }

        public bool TryAccessBuilding(BuildingCore buildingCore)
        {
            return true;
        }

        public void MoveToIdlePositionInBuilding(Transform target)
        {
            float duration = (target.position.x - transform.position.x) / _moveSpeed;
            transform.DOMoveX(target.position.x, duration).OnComplete(() =>
            {
                ChangeState(TroopState.Idle);
            });
        }

        public void MoveOutOfBuilding(EntityDirection direction)
        {
            _moveDirection = direction == EntityDirection.Left ? Vector2.left : Vector2.right;

            ChangeState(TroopState.Moving);
        }



        private void ChangeState(TroopState newState)
        {
            switch (newState)
            {
                case TroopState.Idle:
                    _currentTroopState = new TroopIdleState();
                    break;
                case TroopState.Moving:
                    _currentTroopState = new TroopMoveState();
                    break;
                // case TroopState.Attacking:
                //     _currentTroopState = new TroopAttackState();
                //     break;
                default:
                    _currentTroopState = new TroopIdleState();
                    Debug.Log("Invalid Troop State");
                    break;
            }

            _currentTroopState.ChangeState(_currentTroopState, this);
        }
    }
}
