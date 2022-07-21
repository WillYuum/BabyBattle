using UnityEngine;
using GameplayUtils.Methods;
using System.Collections.Generic;
using Troops;
using DG.Tweening;

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

        [SerializeField] private BoxCollider2D _collider;

        private int enemyCount = 0;
        private int friendCount = 0;
        private bool isInvokeRepeating = false;



        private void Awake()
        {

            UI.SetActive(false);
            _takeOverTerritoryController = new TakeOverTerritoryController();

            if (_controlledBy == ControlledBy.None)
            {
                SetFlagPosition(0);
            }
        }

        private void ToggleInvokeRepeating(bool enable)
        {
            if (enable)
            {
                if (!isInvokeRepeating)
                {
                    InvokeRepeating(nameof(UpdateTerritoryControl), 0.0f, 1.0f);
                    isInvokeRepeating = true;
                }
            }
            else
            {
                if (isInvokeRepeating)
                {
                    CancelInvoke(nameof(UpdateTerritoryControl));
                    isInvokeRepeating = false;
                }
            }
        }


        private void UpdateTerritoryControl()
        {
            if (friendCount > enemyCount)
            {
                _takeOverTerritoryController.GainControl();

            }
            else if (friendCount < enemyCount)
            {
                _takeOverTerritoryController.LoseControl();
            }
            else
            {
                //friendCount == enemyCount
            }


            var controlledBy = _takeOverTerritoryController.GetControlledBy();
            if (controlledBy != ControlledBy.None)
            {
                _controlledBy = controlledBy;
                HandleTerritoryChange(controlledBy);
            }


            UpdateTerritoryVisuals(Mathf.Abs(_takeOverTerritoryController.Porgress));
        }

        private void HandleTerritoryChange(ControlledBy newController)
        {
            bool sameController = _controlledBy == newController;
            if (sameController)
            {
                return;
            }


            float controlRatio = _takeOverTerritoryController.Porgress;
            bool hasControlledByFriends = controlRatio >= 1.0f && newController == ControlledBy.Friend;
            if (hasControlledByFriends)
            {
                _controlledBy = ControlledBy.Friend;
                UnlockTerritoryAbilities();
            }
            else
            {
                bool hasControlledByEnemies = controlRatio <= -1.0f && newController == ControlledBy.Foe;
                if (hasControlledByEnemies)
                {
                    _controlledBy = ControlledBy.Foe;
                    LockTerritoryAbilities();
                }
            }
        }

        private void UpdateTerritoryVisuals(float flagPositionRatio)
        {
            switch (_controlledBy)
            {
                case ControlledBy.Friend:
                    break;
                case ControlledBy.Foe:
                    break;
                case ControlledBy.None:
                    break;
            }

            SetFlagPosition(flagPositionRatio);
        }


        private void SetFlagPosition(float ratio)
        {
            var flagPos = flag.transform.localPosition;
            flagPos.y = Mathf.Lerp(flagPositionPoints[0], flagPositionPoints[1], ratio);
            flag.transform.DOLocalMoveY(flagPos.y, _takeOverTerritoryController.ProgressScaleChange);
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
            Debug.Log("Entering territory");
            if (other.gameObject.TryGetComponent<Troop>(out Troop troop))
            {
                if (troop.FriendOrFoe == FriendOrFoe.Foe)
                {
                    enemyCount++;
                    ToggleInvokeRepeating(true);
                }
                else
                {
                    friendCount++;
                    ToggleInvokeRepeating(true);
                }

                if (_troopsFightingForTerritory.Count < maxTroopFighting)
                {
                    _troopsFightingForTerritory.RemoveAll(v => v == null);
                    if (!_troopsFightingForTerritory.Contains(troop))
                    {
                        _troopsFightingForTerritory.Add(troop);
                        MoveTroopIntoDefensePosition(troop);
                    }
                }

            }
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
            if (other.gameObject.TryGetComponent<Troop>(out Troop troop))
            {
                if (troop.FriendOrFoe == FriendOrFoe.Foe)
                {
                    enemyCount--;
                }
                else
                {
                    friendCount--;
                }
            }

            if (enemyCount == 0 && friendCount == 0)
            {
                ToggleInvokeRepeating(false);
            }
        }

        private const int maxTroopFighting = 3;
        private List<Troop> _troopsFightingForTerritory = new List<Troop>();
        private void MoveTroopIntoDefensePosition(Troop troop)
        {
            if (_troopsFightingForTerritory.Count >= maxTroopFighting) return;

            _troopsFightingForTerritory.Add(troop);

            Vector2 controlPosition = transform.position;
            controlPosition.y = troop.transform.position.y;
            troop.MoveToIdlePositionInBuilding(controlPosition);
        }
    }


    public class TakeOverTerritoryController
    {
        private float _durationToControl = 8.5f;

        //Progress should be betweem -1 and 1
        public float Porgress { get; private set; }

        public float ProgressScaleChange;

        public TakeOverTerritoryController()
        {
            ProgressScaleChange = 1 / _durationToControl;
        }

        public void GainControl()
        {
            Porgress += ProgressScaleChange;

            if (Porgress >= 1)
            {
                Porgress = 1;
            }
        }


        public void LoseControl()
        {
            Porgress -= ProgressScaleChange;

            if (Porgress <= -1)
            {
                Porgress = -1;
            }
        }

        public ControlledBy GetControlledBy()
        {
            switch (Porgress)
            {
                case 1:
                    return ControlledBy.Friend;
                case -1:
                    return ControlledBy.Foe;
                default:
                    return ControlledBy.None;
            }
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