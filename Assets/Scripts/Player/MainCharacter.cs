using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;
using Troops;
using HUDCore;

public class MainCharacter : MonoBehaviourSingleton<MainCharacter>, IDamageable/* , TroopActions */
{
    [SerializeField] private float _moveSpeed = 3.0f;
    private EntityDirection _moveDirection = EntityDirection.Right;
    private bool _shouldMove = false;
    private float _currentHealth = 100.0f;

    [SerializeField] private GameObject _characterVisual;

    public Vector2 GetPos { get { return transform.position; } }

    private void Awake()
    {
        _currentHealth = 100.0f;
        HUD.instance.OnUpdatePlayerHealth.Invoke(_currentHealth / 100.0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var damageAction = new TakeDamageAction { DamageAmount = 10, DamagedByTroop = TroopType.BabyTank };
            TakeDamage(damageAction);
        }
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
        if (_moveDirection == EntityDirection.Left)
        {
            _characterVisual.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            _characterVisual.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }


        _moveDirection = direction;


        float moveDir = direction == EntityDirection.Left ? -1.0f : 1.0f;

        Vector3 move = new Vector3(moveDir * _moveSpeed, 0, 0);
        transform.position += move * Time.deltaTime;
    }

    public void TakeDamage(TakeDamageAction action)
    {
        _currentHealth -= action.DamageAmount;
        HUD.instance.OnUpdatePlayerHealth.Invoke(_currentHealth / 100.0f);

        if (_currentHealth <= 0)
        {
            Invoke(nameof(Die), 1.0f);
        }
    }

    // public void Attack()
    // {
    //     float moveDir = _moveDirection == EntityDirection.Left ? -1.0f : 1.0f;

    //     Vector3 shootDir = new Vector3(moveDir, 0, 0);

    //     RaycastHit2D hit2D = Physics2D.Raycast(transform.position, shootDir, 5.0f, LayerMask.GetMask("Enemy"));

    //     if (hit2D.collider == null) return;

    //     if (hit2D.collider.TryGetComponent<Troop>(out Troop enemy))
    //     {
    //         //FIXME: Damaged by troop should be by player, but do I need to pass the main character troop type?
    //         var damageAction = new TakeDamageAction { DamageAmount = 10, DamagedByTroop = TroopType.LargeBaby };

    //         enemy.TakeDamage(damageAction);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Collectable"))
        {
            GameloopManager.instance.CollectToys(new CollectToysEvent { ToysCount = 1, CollectedToy = other.gameObject });
        }
    }

    public void Die()
    {
        GameloopManager.instance.MainCharacterDied();
    }
}