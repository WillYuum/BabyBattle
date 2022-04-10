using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;


namespace CameraFitCore
{

    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class CameraFit : MonoBehaviour
    {

        [SerializeField] private CameraHeightConfig _hieghtSize;

        void Awake()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += HandleOnPlayModeChanged;
# endif
        }


        //NOTE: Made so I can test the camera fit in the editor
#if UNITY_EDITOR
        [SerializeField] private bool canEdit = false;
        [SerializeField] private PlayerControl _fitToTest;
#endif

        //NOTE: Made so I can test the camera fit in the editor
#if UNITY_EDITOR
        void LateUpdate()
        {
            if (canEdit == false) return;

            var fitToTest = _fitToTest == PlayerControl.None ? _fitToTest = PlayerControl.Camera : _fitToTest;
            Camera.main.orthographicSize = GetFitHeightValue(_hieghtSize.GetHeightValue(fitToTest));
        }
#endif

        public void SwitchToCameraFit(PlayerControl playerControl, float duration = 0.0f)
        {
            float endValue = GetFitHeightValue(_hieghtSize.GetHeightValue(playerControl));

            if (duration == 0.0f)
            {
                Camera.main.orthographicSize = endValue;
            }
            else
            {
                DOTween.To(() => Camera.main.orthographicSize, (x) => Camera.main.orthographicSize = x, endValue, duration);
            }
        }


        private float GetFitHeightValue(float hightSize) => hightSize / 2;



#if UNITY_EDITOR
        private void HandleOnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                canEdit = false;
            }
        }

#endif



    }
}

[System.Serializable]
struct CameraHeightConfig
{
    [SerializeField] private float cameraControlFit;
    [SerializeField] private float mainCharacterControlFit;


    public float GetHeightValue(PlayerControl playerControl)
    {
        if (playerControl == PlayerControl.MainCharacter)
        {
            return mainCharacterControlFit;
        }
        else
        {
            return cameraControlFit;
        }
    }
}