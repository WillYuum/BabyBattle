using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HUD.Screens
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        void Awake()
        {
            gameObject.SetActive(false);
            GameloopManager.instance.OnLoseGame += ShowLoseScreen;

            _restartButton.onClick.AddListener(ClickedOnRestartGame);
        }

        private void ShowLoseScreen()
        {
            gameObject.SetActive(true);
        }

        private void ClickedOnRestartGame()
        {
            GameloopManager.instance.RestartGame();
        }
    }
}
