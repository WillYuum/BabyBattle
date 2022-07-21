using System.Collections;
using System.Collections.Generic;
using Buildings;
using DG.Tweening;
using UnityEngine;

public interface IGarrisonable
{
    void GarrisonTroops(Troops.Troop troop);
    void UnGarrisonTroops();
}




namespace Troops
{
    using States;

    public enum TroopType { SharpShooter, BabyTank, MortarBaby };
    public enum TroopState { Idle, Moving, Attacking };


    public interface ITroopBuildingInteraction
    {
        void MoveToIdlePositionInBuilding(Vector3 targetPosition);
        void MoveOutOfBuilding(EntityDirection direction);
    }



    public abstract class Troop : MonoBehaviour, IDamageable, ITroopBuildingInteraction
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
        public Animator Animator { get; private set; }

        [SerializeField] private GameObject _characterVisual;


        private void Awake()
        {
            Animator = gameObject.GetComponent<Animator>();

#if UNITY_EDITOR
            if (Animator == null) Debug.LogError("Animator is null");
#endif
        }

        void Update()
        {
            _currentTroopState?.Execute();
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

            Debug.DrawLine(transform.position, transform.position + (Vector3)MoveDirection * attackDistance, Color.red);

            if (collider == null || collider.gameObject == gameObject) return;


            if (collider.GetComponent<IDamageable>() != null)
            {
                if (collider.TryGetComponent<Troop>(out Troop troop))
                {
                    if (troop.FriendOrFoe != FriendOrFoe)
                    {
                        if (TroopState == TroopState.Attacking) return;

                        print(FriendOrFoe + "Troop found " + troop.FriendOrFoe);
                        ChangeState(TroopState.Attacking);
                    }
                }
                else
                {
                    if (collider.TryGetComponent<BuildingCore>(out BuildingCore building))
                    {
                        if (building.FriendOrFoe != FriendOrFoe)
                        {
                            if (TroopState == TroopState.Attacking) return;

                            print(FriendOrFoe + "Building found " + building.FriendOrFoe);
                            ChangeState(TroopState.Attacking);
                        }
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
            print("Troop died " + gameObject.name);
            Destroy(gameObject, 0.0f);
        }

        protected void SetMoveDirection(EntityDirection direction)
        {
            (Vector2 moveDir, Vector3 scale) = direction switch
            {
                EntityDirection.Left => (Vector2.left, new Vector3(-1, 1, 1)),
                EntityDirection.Right => (Vector2.right, new Vector3(1, 1, 1)),
                EntityDirection.Idle => (MoveDirection, new Vector3(MoveDirection.x, MoveDirection.y, 1)),
                _ => throw new System.Exception("Invalid direction: " + direction),
            };

            _characterVisual.transform.localScale = scale;
            _existenceCollider.SwitchDirection(direction);
            MoveDirection = moveDir;
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

        //This should be private all the time
        private void GetControlledByGod(TroopState ControlledState, EntityDirection direction)
        {
            _currentTroopState = new ControlledByGodState();
            TroopState = ControlledState;
            SetMoveDirection(direction);

            //Change state should happen in the end
            _currentTroopState.ChangeState(_currentTroopState, this);
        }

        public void SetCurrentMoveSpeed(float speed)
        {
            CurrentMoveSpeed = speed;
        }

        protected void UpdateMoveSpeedOnTroopBehind()
        {
            // if (_troopBehind != null)
            // {
            //     _troopBehind.SetCurrentMoveSpeed(_troopBehind.DefaultMoveSpeed);
            // }
        }

        public void MoveToIdlePositionInBuilding(Vector3 targetPosition)
        {
            var direction = targetPosition.x > 0 ? EntityDirection.Right : EntityDirection.Left;
            GetControlledByGod(TroopState.Moving, direction);
            transform.DOMove(targetPosition, 0.5f).OnComplete(() =>
            {
                ChangeState(TroopState.Idle);
            });
        }

        public void MoveOutOfBuilding(EntityDirection direction)
        {
            SetMoveDirection(direction);
            ChangeState(TroopState.Moving);
        }


        //NOTE: For testing in editor
#if UNITY_EDITOR

        [ContextMenu("Force Init State")]
        private void ForceInitState()
        {
            InitTroop(EntityDirection.Left);
        }

        [ContextMenu("Set dir to left")]
        private void SetDirToLeft()
        {
            SetMoveDirection(EntityDirection.Left);
        }

        [ContextMenu("Set dir to right")]
        private void SetDirToRight()
        {
            SetMoveDirection(EntityDirection.Right);
        }

        [ContextMenu("Set To Attack State")]
        private void SetToAttackState()
        {
            ChangeState(TroopState.Attacking);
        }

        [ContextMenu("Set To Idle State")]
        private void SetToIdleState()
        {
            ChangeState(TroopState.Idle);
        }

        [ContextMenu("Set To Moving State")]
        private void SetToMovingState()
        {
            ChangeState(TroopState.Moving);
        }
#endif
    }
}