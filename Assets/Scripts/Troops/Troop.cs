using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Troops
{
    public enum TroopType { BabyTroop, LargeBaby, MortarBaby };
    public enum TroopState { Idle, Moving, Attacking, Dead };

    public interface TroopActions
    {
        void Move();
        void TakeDamage(TroopTakeDamageAction action);
        void Attack();
        void Die();
    }

    public struct TroopTakeDamageAction
    {
        public float DamageAmount;
        public TroopType DamagedByTroop;
        public TroopTakeDamageAction(int damageAmount, TroopType damagedByTroop)
        {
            DamageAmount = damageAmount;
            DamagedByTroop = damagedByTroop;
        }
    }


    public class Troop : MonoBehaviour, TroopActions
    {
        protected TroopType _troopType;
        protected float _currentHealth { get; private set; }
        protected float _attackDelay { get; private set; }
        protected float _moveSpeed { get; private set; }
        protected float _attackDamage { get; private set; }
        protected Vector2 _moveDirection { get; private set; }

        public void InitTroop(EntityDirection moveDir)
        {
            _moveSpeed = 5.0f;
            _moveDirection = moveDir == EntityDirection.Left ? Vector2.left : Vector2.right;
        }

        public virtual void Attack()
        {
            var damageAction = new TroopTakeDamageAction
            {
                DamageAmount = _attackDamage,
                DamagedByTroop = _troopType,
            };


            //Raycast towards direction and check if target is hit

            //If target is hit, get the target's IDamageable interface and call TakeDamage() and pass the  damage action
        }

        void Update()
        {
            Move();
        }

        public virtual void Move()
        {
            transform.position += (Vector3)_moveDirection * _moveSpeed * Time.deltaTime;
        }

        public virtual void TakeDamage(TroopTakeDamageAction damageAction)
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
    }
}