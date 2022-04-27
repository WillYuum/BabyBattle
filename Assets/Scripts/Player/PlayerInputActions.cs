using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;
using Player.Controls;
using UnityEngine.EventSystems;
using System.Linq;
using SpawnManagerCore;

namespace Player.InputsController
{
    public class PlayerInputActions : MonoBehaviour
    {
        private TryingToSpawnCharacter _tryingToSpawnTroop = new TryingToSpawnCharacter();
        private PlayerIdleState _playerIdleState = new PlayerIdleState();
        private CanSpawnTroopState _canSpawnTroopState = new CanSpawnTroopState();

        private PlayerInputState _currentPlayerInput;

        private GameObject _spawnedTroop;
        public float MinDistanceToSpawnTroop = 1f;
        public Vector3 StartingMousePos { get; private set; }


        void Awake()
        {
            _tryingToSpawnTroop.Init(this);
            _playerIdleState.Init(this);
            _canSpawnTroopState.Init(this);


            _currentPlayerInput = _playerIdleState;
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
            print("CLICKED ON " + EventSystem.current.currentSelectedGameObject.name);
            if (EventSystem.current.currentSelectedGameObject.CompareTag("Card"))
            {
                var card = EventSystem.current.currentSelectedGameObject.GetComponent<InteractableUI.TroopCard>();


                _spawnedTroop = SpawnManager.instance.SpawnFriendlyTroop(card.TroopType).gameObject;
                StartingMousePos = Input.mousePosition;

                _currentPlayerInput = _tryingToSpawnTroop;
            }
        }

        public void OnSwipeWithSpawnCharacter()
        {
            if (_spawnedTroop == null) return;

            Vector3 newTroopPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newTroopPos.z = 0;
            _spawnedTroop.transform.position = newTroopPos;
            // _currentPlayerInput = _canSpawnTroopState;

        }


        public void EnterTryingToSpawnTroopState()
        {
            _currentPlayerInput = _tryingToSpawnTroop;

            //Maybe switch from troop visual to card visual
        }

        public void EnterCanSpawnTroopState()
        {
            _currentPlayerInput = _canSpawnTroopState;
        }

        public void InvokeSpawnTroop()
        {
            if (_spawnedTroop == null) return;

            _currentPlayerInput = _playerIdleState;
            _spawnedTroop = null;
        }


        public void HandlePlayerClickedOnMouse()
        {
            MainCharacter.instance.Attack();
        }


        public void ClickedOnSwitchPlayerControl(PlayerControl playerControl)
        {
            GameloopManager.instance.SwitchPlayerControl();
        }
    }
}