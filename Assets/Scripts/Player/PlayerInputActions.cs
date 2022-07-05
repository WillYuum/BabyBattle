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
            //Check if clicked on maincamp
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Debug.DrawRay(mousePos, Vector2.zero, Color.green, 5f);
            var hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "MainCamp")
                {
                    //Switch main camp attack position
                }
            }
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