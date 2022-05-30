using UnityEngine;
using DG.Tweening;
using CameraFitCore;
using CameraControllerCore.States;

namespace CameraControllerCore
{
    public class CameraController : MonoBehaviour
    {
        private CameraFollowPlayerState _followPlayerCameraState = new CameraFollowPlayerState();
        private CameraStateCore _currentCameraState;


        private Vector3 _cameraMinPosition;
        private Vector3 _cameraMaxPosition;


        void Awake()
        {
            SetCameraPositionBound();

            _currentCameraState = _followPlayerCameraState;
            _followPlayerCameraState.Init(this);
        }

        void Update()
        {
            _currentCameraState.Execute();
        }


        private void SetCameraPositionBound()
        {
            Vector2 cameraCurrentPosition = Camera.main.transform.position;
            float cameraHalfWidthLength = Camera.main.ViewportToWorldPoint(new Vector2(1.0f, 0f)).x;
            //Need to do the calculation when camera is set in origin position
            //so removing unwanted extra length
            cameraHalfWidthLength -= cameraCurrentPosition.x;

            var worldBorders = WorldManager.instance.WorldBorders;
            _cameraMinPosition = worldBorders.MaxLeft.position;
            _cameraMaxPosition = worldBorders.MaxRight.position;

            void addOffsetToMinMaxPosition(ref Vector3 bounds, float offset)
            {
                bounds += new Vector3(offset, 0f, 0f);
            }

            addOffsetToMinMaxPosition(ref _cameraMinPosition, cameraHalfWidthLength);
            addOffsetToMinMaxPosition(ref _cameraMaxPosition, -cameraHalfWidthLength);
        }



        public void CameraFollowPlayer()
        {
            Vector3 newCameraPos = MainCharacter.instance.GetPos;

            newCameraPos.x = Mathf.Clamp(newCameraPos.x, _cameraMinPosition.x, _cameraMaxPosition.x);
            newCameraPos.y = transform.position.y;
            newCameraPos.z = transform.position.z;

            transform.position = newCameraPos;
        }
    }
}