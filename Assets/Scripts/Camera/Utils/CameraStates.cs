using UnityEngine;

namespace CameraControllerCore.States
{

    public class MainCharacterCameraState : CameraStateCore
    {
        public override void Execute()
        {
            base.Execute();

            Vector3 newCameraPos = MainCharacter.instance.GetPos;
            newCameraPos.z = _cameraController.transform.position.z;
            _cameraController.transform.position = newCameraPos;
        }
    }


    [System.Serializable]
    public class ControlCameraState : CameraStateCore
    {
        [SerializeField] private float _edgeLengthToScroll = 25;
        [SerializeField] private float _moveSpeed = 5.0f;
        public override void Execute()
        {
            base.Execute();

            Vector3 newCamPos = _cameraController.transform.position;

            if (Input.mousePosition.x > Screen.width - _edgeLengthToScroll)
            {
                newCamPos.x += _moveSpeed * Time.deltaTime;
            }
            else if (Input.mousePosition.x < _edgeLengthToScroll)
            {
                newCamPos.x -= _moveSpeed * Time.deltaTime;
            }

            //NOTE: To keep camera in the correct z-Index 
            newCamPos.z = _cameraController.transform.position.z;

            _cameraController.transform.position = newCamPos;
        }
    }

    [System.Serializable]
    public abstract class CameraStateCore
    {
        protected CameraController _cameraController;

        public void Init(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        public virtual void Execute()
        {

        }
    }
}