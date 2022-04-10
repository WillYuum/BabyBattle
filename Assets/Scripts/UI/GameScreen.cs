using GameplayUI.Helpers;
using TMPro;
using UnityEngine;
namespace HUDCore.Screens
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private HealthBarUI _playerHealthBar;

        [SerializeField] private TextMeshProUGUI tabKeyText;

        void Awake()
        {
            InitGameScreen();
        }

        public void InitGameScreen()
        {
            HUD.instance.OnUpdatePlayerHealth += UpdatePlayerHealth;

            GameloopManager.instance.OnSwitchPlayerControl += SwitchUIForPlayerControl;

            _playerHealthBar.Init();
        }

        private void UpdatePlayerHealth(float health)
        {
            _playerHealthBar.SetHealth(health);
        }

        private void SwitchUIForPlayerControl(PlayerControl playerControl)
        {
            if (playerControl == PlayerControl.Camera)
            {
                tabKeyText.text = "Switch to:\nMainCharacter";
            }
            else
            {
                tabKeyText.text = "Switch to:\nCamera";
            }
        }

    }
}