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


        [Header("References")]
        [SerializeField] private Transform _cameraMinPosition;
        [SerializeField] private Transform _cameraMaxPosition;


        void Awake()
        {

            Vector2 cameraCurrentPosition = Camera.main.transform.position;
            float cameraHalfWidthLength = Camera.main.ViewportToWorldPoint(new Vector2(1.0f, 0f)).x;
            //Need to do the calculation when camera is set in origin position
            //so removing unwanted extra length
            cameraHalfWidthLength -= cameraCurrentPosition.x;

            void addOffsetToMinMaxPosition(Transform bounds, float offset)
            {
                bounds.position += new Vector3(offset, 0f, 0f);
            }

            addOffsetToMinMaxPosition(_cameraMinPosition, cameraHalfWidthLength);
            addOffsetToMinMaxPosition(_cameraMaxPosition, -cameraHalfWidthLength);




            _currentCameraState = _followPlayerCameraState;
            _followPlayerCameraState.Init(this);
        }

        private void SetCameraOffSetToBoundsOnCamera(Transform bounds, Vector3 cameraOffset)
        {
            Vector3 positions = bounds.position;
            positions.x += cameraOffset.x;
            bounds.position = positions;
        }


        void Update()
        {
            _currentCameraState.Execute();
        }

        public void CameraFollowPlayer()
        {
            Vector3 newCameraPos = MainCharacter.instance.GetPos;

            newCameraPos.x = Mathf.Clamp(newCameraPos.x, _cameraMinPosition.position.x, _cameraMaxPosition.position.x);
            newCameraPos.y = transform.position.y;
            newCameraPos.z = transform.position.z;

            transform.position = newCameraPos;
        }
    }
}