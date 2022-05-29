using GameplayUI.Helpers;
using TMPro;
using UnityEngine;
namespace HUDCore.Screens
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private HealthBarUI _playerHealthBar;

        [SerializeField] private TextMeshProUGUI tabKeyText;
        [SerializeField] private TextMeshProUGUI toysCountText;

        [SerializeField] private TextMeshProUGUI troopsSpawnCountText;

        void Awake()
        {
            InitGameScreen();
            HUD.instance.OnUpdateToysCount += UpdateToysCout;
            HUD.instance.OnUpdateTroopsSpawnCount += UpdateTroopSpawnCount;
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


        private void UpdateToysCout()
        {
            int usedToys = GameloopManager.instance.HoldingToysCount;
            int maxToys = GameloopManager.instance.MaxHoldingToysCount;

            toysCountText.text = $"{usedToys}/{maxToys}";
        }


        public void UpdateTroopSpawnCount()
        {
            int spawnedTroops = GameloopManager.instance.CurrentSpawnedTroopsCount;
            int maxTroops = GameloopManager.instance.MaxSpawedTroopsCount;

            troopsSpawnCountText.text = $"{spawnedTroops}/{maxTroops}";
        }

    }
}