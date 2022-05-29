using System.Collections;
using System.Collections.Generic;
using Buildings;
using DG.Tweening;
using UnityEngine;
namespace Troops
{
    using States;

    public enum TroopType { SharpShooter, BabyTank, MortarBaby };
    public enum TroopState { Idle, Moving, Attacking };

    public interface ITroopBuildingInteraction
    {
        bool TryAccessBuilding(BuildingCore buildingCore);
        void MoveToIdlePositionInBuilding(Transform target);
        void MoveOutOfBuilding(EntityDirection direction);
    }



    public abstract class Troop : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public TroopType TroopType { get; private set; }
        [field: SerializeField] public FriendOrFoe FriendOrFoe { get; private set; }
        public float CurrentHealth { get; private set; }
        public float AttackDelay { get; private set; }
        public float DefaultMoveSpeed { get; private set; }
        public float CurrentMoveSpeed { get; private set; }
        public float AttackDamage { get; private set; }
        public Vector2 MoveDirection { get; private set; }
        public float attackDistance { get; private set; }

        private Troop _troopBehind;


        public TroopState TroopState { get; private set; }
        private TroopStateCore _currentTroopState;

        private ExistenceCollider _existenceCollider;

        [SerializeField] private GameObject _characterVisual;

        void Update()
        {
            _currentTroopState.Execute();
        }


        public void InitTroop(EntityDirection moveDir)
        {
            TroopVariable data = GameVariables.Instance.TroopVariables.GetVariable(TroopType);

            CurrentHealth = data.StartingHealth;
            AttackDelay = data.AttackDelay;
            DefaultMoveSpeed = data.MoveSpeed;
            CurrentMoveSpeed = DefaultMoveSpeed;
            AttackDamage = data.Damage;

            attackDistance = 1f;

            //NOTE: This is a temp solution, need to be changed for scale
            _existenceCollider = transform.Find("ExistenceCollider").GetComponent<ExistenceCollider>();

            SetMoveDirection(moveDir);

            ChangeState(TroopState.Moving);
        }


        public abstract void Attack();
        public void FindTargetToAttack()
        {
            var layer = (1 << LayerMask.NameToLayer("Troop") | (1 << LayerMask.NameToLayer("Building")));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, MoveDirection, attackDistance, layer);
            Collider2D collider = hit.collider;
            if (collider != null)
            {
                if (collider.TryGetComponent<Troop>(out Troop troop))
                {
                    if (troop.FriendOrFoe != FriendOrFoe)
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
                UpdateMoveSpeedOnTroopBehind();
                Die();
            }
        }


        public virtual void Die()
        {
            Destroy(gameObject, 1.0f);
        }

        protected void SetMoveDirection(EntityDirection direction)
        {
            _characterVisual.transform.localScale = new Vector3(direction == EntityDirection.Left ? -1.0f : 1.0f, 1.0f, 1.0f);
            _existenceCollider.SwitchDirection(direction);
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

        protected void UpdateMoveSpeedOnTroopBehind()
        {
            if (_troopBehind != null)
            {
                _troopBehind.SetCurrentMoveSpeed(_troopBehind.DefaultMoveSpeed);
            }
        }
    }
}
