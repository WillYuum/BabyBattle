using UnityEngine;

namespace CameraControllerCore.States
{
    // public class CameraFollowPlayerState : CameraStateCore
    // {
    //     public override void Execute()
    //     {

    //     }
    // }

    public class NormalCameraControlState : CameraStateCore
    {
        public override void Execute()
        {

        }
    }

    public class NoCameraControlState : CameraStateCore
    {
        public override void Execute()
        {

        }
    }


    public abstract class CameraStateCore
    {
        protected CameraController _cameraController;

        public void Init(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        public abstract void Execute();
    }
}