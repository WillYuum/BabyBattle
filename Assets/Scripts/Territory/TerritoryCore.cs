using UnityEngine;
using GameplayUtils.Methods;

namespace Territory
{
    public enum ControlledBy { None, Friend, Foe };
    public class TerritoryCore : MonoBehaviour
    {
        [SerializeField] private ControlledBy _controlledBy = ControlledBy.None;
        private TakeOverTerritoryController _takeOverTerritoryController;

        [Tooltip("First index is starting position & second index is ending position")]
        [SerializeField] private float[] flagPositionPoints;
        [SerializeField] private GameObject flag;

        [SerializeField] private GameObject UI;


        private void Awake()
        {

            UI.SetActive(false);
            _takeOverTerritoryController = new TakeOverTerritoryController();

            if (_controlledBy == ControlledBy.None)
            {
                SetFlagPosition(0);
            }
        }



        public void TryTakeControl(ControlledBy beingControlledBy)
        {
            if (beingControlledBy == _controlledBy)
            {
                return;
            }


            if (_controlledBy == ControlledBy.None)
            {
                //Gain control
                _takeOverTerritoryController.GainControl();
            }
            else
            {
                //Lose control
                _takeOverTerritoryController.LoseControl();
            }


            float controlRatio = _takeOverTerritoryController.GetControlledRation();
            if (controlRatio >= 1)
            {
                _controlledBy = beingControlledBy;

                if (beingControlledBy == ControlledBy.Friend)
                {
                    UnlockTerritoryAbilities();
                }
            }
            else if (controlRatio < 0)
            {
                _controlledBy = ControlledBy.None;

                if (beingControlledBy == ControlledBy.Foe)
                {
                    LockTerritoryAbilities();
                }
            }

            SetFlagPosition(controlRatio);
        }


        private void SetFlagPosition(float ratio)
        {
            var flagPos = flag.transform.localPosition;
            flagPos.y = Mathf.Lerp(flagPositionPoints[0], flagPositionPoints[1], ratio);
            flag.transform.localPosition = flagPos;
        }

        private void UnlockTerritoryAbilities()
        {
            print("Unlocking territory abilities");
        }


        private void LockTerritoryAbilities()
        {
            print("Locking territory abilities");
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (UtilMethods.CollidedWithPlayer(other))
            {
                //show UI
                UI.SetActive(true);

                GameloopManager.instance.UpdateHoveredTerritoryState(this);
            }
        }


        void OnTriggerExit2D(Collider2D other)
        {
            if (UtilMethods.CollidedWithPlayer(other))
            {
                //hide UI
                UI.SetActive(false);

                GameloopManager.instance.UpdateHoveredTerritoryState(null);
            }
        }
    }


    public class TakeOverTerritoryController
    {
        private float _durationToControl = 5f;
        private float _currentDurationTillControl = 0f;

        public void GainControl()
        {
            _currentDurationTillControl += Time.deltaTime;
        }


        public void LoseControl()
        {
            _currentDurationTillControl -= Time.deltaTime;
        }

        public float GetControlledRation()
        {
            return _currentDurationTillControl / _durationToControl;
        }
    }
}