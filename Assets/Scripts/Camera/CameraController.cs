using UnityEngine;
using DG.Tweening;
using CameraFitCore;
using CameraControllerCore.States;

namespace CameraControllerCore
{
    public class CameraController : MonoBehaviour
    {
        private CameraFit _cameraFit;


        [Header("Camera States")]
        [SerializeField] private ControlCameraState _controlCameraState = new ControlCameraState();
        [SerializeField] private MainCharacterCameraState _followMainCharacterCameraState = new MainCharacterCameraState();
        private CameraStateCore _currentCameraState;



        void Awake()
        {

        }


        void Update()
        {
        }


        private void SwitchCameraFOVToPlayerControl(PlayerControl playerControl)
        {
            float tweenDuration = 0.5f;

            if (playerControl == PlayerControl.MainCharacter)
            {
                float mainCharacterXPos = MainCharacter.instance.transform.position.x;

                _cameraFit.SwitchToCameraFit(GameloopManager.instance.PlayerControl, tweenDuration);
                transform.DOMoveX(mainCharacterXPos, tweenDuration);

                _currentCameraState = _followMainCharacterCameraState;
            }
            else
            {
                _cameraFit.SwitchToCameraFit(GameloopManager.instance.PlayerControl, tweenDuration);

                _currentCameraState = _controlCameraState;
            }
        }
    }
}