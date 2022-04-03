using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;

public enum PlayerControl { MainCharacter, Camera };

public class MainCharacter : MonoBehaviourSingleton<MainCharacter>, TroopActions
{
    [SerializeField] private float _moveSpeed = 3.0f;
    private EntityDirection _moveDirection = EntityDirection.Right;
    private bool _shouldMove = false;
    private float _currentHealth = 100.0f;

    private void Awake()
    {

    }

    public void HandleShoot(EntityDirection direction)
    {
        float shootDir = direction == EntityDirection.Left ? -1.0f : 1.0f;
    }

    public void TogglePlayerMovement(bool shouldMove)
    {
        _shouldMove = shouldMove;
        if (_moveDirection == EntityDirection.Left)
        {
            _moveDirection = EntityDirection.Right;
        }
        else
        {
            _moveDirection = EntityDirection.Left;
        }
    }

    public void Move(EntityDirection direction)
    {
        _moveDirection = direction;
        float moveDir = direction == EntityDirection.Left ? -1.0f : 1.0f;

        Vector3 move = new Vector3(moveDir * _moveSpeed, 0, 0);
        transform.position += move * Time.deltaTime;
    }

    public void TakeDamage(TroopTakeDamageAction action)
    {
        _currentHealth -= action.damageAmount;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Attack()
    {
        float moveDir = _moveDirection == EntityDirection.Left ? -1.0f : 1.0f;

    }

    public void Die()
    {
        GameloopManager.instance.MainCharacterDied();
    }
}