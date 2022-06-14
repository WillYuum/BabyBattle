using UnityEngine;
using DG.Tweening;
using CameraFitCore;
using CameraControllerCore.States;

namespace CameraControllerCore
{
    public class CameraController : MonoBehaviour
    {

        private Vector3 _cameraMinPosition;
        private Vector3 _cameraMaxPosition;


        void Awake()
        {
            CalculateCameraPositionBound();
        }


        public void MoveCamera(EntityDirection direction)
        {
            Vector3 newCamPos = transform.position;
            switch (direction)
            {
                case EntityDirection.Left:
                    newCamPos += new Vector3(-1, 0, 0);
                    break;
                case EntityDirection.Right:
                    newCamPos += new Vector3(1, 0, 0);
                    break;
            }

            newCamPos.x = Mathf.Clamp(newCamPos.x, _cameraMinPosition.x, _cameraMaxPosition.x);
            transform.position = newCamPos;
        }

        private void CalculateCameraPositionBound()
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

    }
}