using UnityEngine;
using Player.Controls;

namespace Player.InputsController
{
    public enum PlayerInputState { Normal, InTerritoryAreaInputs, };
    public class PlayerInputActions : MonoBehaviour
    {

        private PlayerIdleState _playerIdleState = new PlayerIdleState();
        private MainCharacterIdleState _mainCharacterIdleState = new MainCharacterIdleState();
        private InTerritoryAreaInputs _inTerritoryAreaInputs = new InTerritoryAreaInputs();

        private PlayerInputStateCore _currentPlayerInput;

        private GameObject _spawnedTroop;
        public float MinDistanceToSpawnTroop = 1f;
        public Vector3 StartingMousePos { get; private set; }


        void Awake()
        {
            _playerIdleState.Init(this);
            _mainCharacterIdleState.Init(this);
            _inTerritoryAreaInputs.Init(this);


            SwitchToState(PlayerInputState.Normal);
            // GameloopManager.instance.OnSwitchPlayerControl += OnPlayerControlSwitch;
        }

        void Update()
        {
            _currentPlayerInput.CheckInput();
        }


        public void HandlePlayerMove(EntityDirection direction)
        {
            MainCharacter.instance.Move(direction);
        }


        public void EnterIdleStateWhileTryingToSpawnTroop()
        {
            _currentPlayerInput = _playerIdleState;
        }

        public void OnClickInIdle()
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f, LayerMask.GetMask("Building"));
            if (hit.collider)
            {
                if (hit.collider.TryGetComponent<MainCamp>(out var mainCamp))
                {
                    //Switch direction of spawning troops
                }

                return;
            }

            // if (EventSystem.current.currentSelectedGameObject.CompareTag("Card"))
            // {
            //     var card = EventSystem.current.currentSelectedGameObject.GetComponent<InteractableUI.TroopCard>();


            //     _spawnedTroop = SpawnManager.instance.SpawnFriendlyTroop(card.TroopType).gameObject;
            //     StartingMousePos = Input.mousePosition;
            // }
        }


        public void SwitchToState(PlayerInputState newInputState)
        {
            switch (newInputState)
            {
                case PlayerInputState.Normal:
                    _currentPlayerInput = _mainCharacterIdleState;
                    break;
                case PlayerInputState.InTerritoryAreaInputs:
                    _currentPlayerInput = _inTerritoryAreaInputs;
                    break;
                default:
                    Debug.LogError("Unknown player input state");
                    break;
            }
        }
    }
}