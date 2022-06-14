using UnityEngine;
using Player.Controls;


public enum PlayerControlState { Normal, Locked }
namespace Player.InputsController
{
    public enum PlayerInputState { Normal, Locked };
    public class PlayerInputActions : MonoBehaviour
    {

        private NormalPlayerInput _normalPlayerInput = new NormalPlayerInput();
        private LockedPlayerInput _lockedPlayerInput = new LockedPlayerInput();
        private PlayerInputStateCore _currentPlayerInput;

        public CameraControllerCore.CameraController CameraController { get; private set; }

        void Awake()
        {
            _normalPlayerInput.Init(this);
            _lockedPlayerInput.Init(this);

            SwitchToState(PlayerControlState.Normal);
            CameraController = Camera.main.GetComponent<CameraControllerCore.CameraController>();
            // GameloopManager.instance.OnSwitchPlayerControl += OnPlayerControlSwitch;
        }

        void Update()
        {
            _currentPlayerInput.CheckInput();
        }


        public void InvokeClickScreen()
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

        public void InvokeMoveCamera(EntityDirection direction)
        {
            CameraController.MoveCamera(direction);
        }


        public void SwitchToState(PlayerControlState newInputState)
        {
            switch (newInputState)
            {
                case PlayerControlState.Normal:
                    _currentPlayerInput = _normalPlayerInput;
                    break;
                case PlayerControlState.Locked:
                    _currentPlayerInput = _lockedPlayerInput;
                    break;
                default:
                    Debug.LogError("Unknown player input state");
                    break;
            }
        }
    }
}