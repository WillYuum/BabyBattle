using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TroopType { BabyTroop, LargeBaby, MortarBaby };
public enum TroopState { Idle, Moving, Attacking, Dead };
public enum EntityDirection { Left, Right };

public interface TroopActions
{
    void Move(EntityDirection direction);
    void TakeDamage(TroopTakeDamageAction action);
    void Attack();
    void Die();
}


public class Troop : MonoBehaviour, TroopActions
{
    protected TroopType _troopType;
    protected float _currentHealth { get; private set; }
    protected float _attackDelay { get; private set; }
    protected float _moveSpeed { get; private set; }
    protected float _attackDamage { get; private set; }
    protected Vector2 _moveDirection { get; private set; }


    public virtual void Attack()
    {
        var damageAction = new TroopTakeDamageAction
        {
            damageAmount = _attackDamage,
            damagedByTroop = _troopType,
        };


        //Raycast towards direction and check if target is hit

        //If target is hit, get the target's IDamageable interface and call TakeDamage() and pass the  damage action
    }

    public virtual void Move(EntityDirection troopDirection)
    {

    }

    public virtual void TakeDamage(TroopTakeDamageAction damageAction)
    {
        _currentHealth -= damageAction.damageAmount;
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




public struct TroopTakeDamageAction
{
    public float damageAmount;
    public TroopType damagedByTroop;
}