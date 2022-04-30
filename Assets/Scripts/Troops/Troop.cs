using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Troops
{

    public enum TroopType { BabyTroop, LargeBaby, MortarBaby };
    public enum TroopState { Idle, Moving, Attacking, Dead };


    public interface ITroopActions
    {
        void Move();
        void TakeDamage(TakeDamageAction action);
        void Attack();
        void Die();
    }



    public class Troop : MonoBehaviour, ITroopActions, IDamageable
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
            Move();
        }

        public void CheckForEnemies()
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

        public virtual void Move()
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
    }
}