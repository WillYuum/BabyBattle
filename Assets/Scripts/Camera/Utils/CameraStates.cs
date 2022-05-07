using UnityEngine;

namespace CameraControllerCore.States
{
    public class CameraFollowPlayerState : CameraStateCore
    {
        public override void Execute()
        {
            base.Execute();

            _cameraController.CameraFollowPlayer();
        }
    }


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