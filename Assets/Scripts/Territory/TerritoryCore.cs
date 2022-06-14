using UnityEngine;
using GameplayUtils.Methods;
using System.Collections.Generic;

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


        [Header("Territory Abilities")]
        [SerializeField] private GainMoreTroopsAbility[] troopAbility;
        [SerializeField] private GainToysAbility[] toyGeneratorAbility;


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

            foreach (ITerritoryAbility ability in troopAbility)
            {
                ability.InvokeAbility();
            }

            foreach (ITerritoryAbility ability in toyGeneratorAbility)
            {
                ability.InvokeAbility();
            }
        }


        private void LockTerritoryAbilities()
        {
            print("Locking territory abilities");

            foreach (ITerritoryAbility ability in troopAbility)
            {
                ability.RemoveAbility();
            }

            foreach (ITerritoryAbility ability in toyGeneratorAbility)
            {
                ability.RemoveAbility();
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            // if (UtilMethods.CollidedWithPlayer(other))
            // {
            //     //show UI
            //     UI.SetActive(true);

            // GameloopManager.instance.UpdateHoveredTerritoryState(this);

            //     CollectNewToys();
            // }
        }

        private void CollectNewToys()
        {
            foreach (var ability in toyGeneratorAbility)
            {
                GameloopManager.instance.CollectToys(new CollectToysEvent
                {
                    ToysCount = ability.GetRewards(),
                });
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            // if (UtilMethods.CollidedWithPlayer(other))
            // {
            //     //hide UI
            //     UI.SetActive(false);

            //     GameloopManager.instance.UpdateHoveredTerritoryState(null);
            // }
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


public interface ITerritoryAbility
{
    void InvokeAbility();
    void RemoveAbility();
    int GetRewards();
}


[System.Serializable]
public class GainMoreTroopsAbility : ITerritoryAbility
{
    [SerializeField] private int _amountToGain = 1;

    public int GetRewards()
    {
        return _amountToGain;
    }

    public void InvokeAbility()
    {
        GameloopManager.instance.IncreaseMaxAmountOfTroops(_amountToGain);
    }

    public void RemoveAbility()
    {
        GameloopManager.instance.DecreaseMaxAmountOfTroops(_amountToGain);
    }

}



public interface IToyGeneratorMethods
{
    int ToysGeneratedPerSec { get; set; }
    void GenerateToys(int amount);
}

[System.Serializable]
public class GainToysAbility : ITerritoryAbility, IToyGeneratorMethods
{
    [field: SerializeField] public int ToysGeneratedPerSec { get; set; }
    private int collectedToys = 0;


    public void InvokeAbility()
    {
        GameloopManager.instance.ToyGenerator.Add(this);
    }

    public void RemoveAbility()
    {
        collectedToys = 0;
        GameloopManager.instance.ToyGenerator.Remove(this);
    }

    public void GenerateToys(int amount)
    {
        Debug.Log("Generating toys");
        collectedToys += amount;
    }

    public int GetRewards()
    {
        return collectedToys;
    }
}