using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace GameplayUI.Helpers
{
    public class HealthBarUI : MonoBehaviour
    {
        private Slider _slider;

        public void Init()
        {
            _slider = GetComponent<Slider>();
        }


        /// <summary>
        /// <param> healthRatio: Value should be between 0 and 1 so it can diplay properly on UI</param>
        /// </summary>
        public void SetHealth(float healthRatio, float tweenToDuration = 0f)
        {
#if UNITY_EDITOR
            if (healthRatio > 1)
            {
                Debug.LogError("Health ratio is greater than 1, make sure you are not passing in a value greater than 1");
                Debug.LogError("Issue happened in: " + gameObject.name + " with value of: " + healthRatio);
            }

            if (_slider == null)
            {
                Debug.LogError("Slider is null, make sure you have initialized the slider");
                Init();
            }
#endif

            _slider.DOValue(healthRatio, tweenToDuration);
        }
    }
}