using System.Collections.Generic;
using GameplayUtils.Methods;
using Troops;
using UnityEngine;
using Utils.ArrayUtils;


namespace Buildings
{
    public enum FriendOrFoe { None, Friend, Foe };
    public enum BuildingType { Buildable, TroopCamp, DefensiveWall };

    public class BuildingCore : MonoBehaviour
    {
        protected FriendOrFoe _friendOrFoe;
        [SerializeField] protected BuildingType _buildingType;

        [SerializeField] protected BuildingUI _buildingUI;

        void Awake()
        {
            OnAwake();

        }

        protected virtual void OnAwake()
        {

        }


        public void Init(ConstructBuildingAction constructBuildingAction)
        {
            // _friendOrFoe = constructBuildingAction.FriendOrFoe;

        }


        public virtual void DestroyBuilding()
        {
            Destroy(gameObject, 2.0f);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (UtilMethods.CollidedWithPlayer(other))
            {
                _buildingUI.ToggleBuildingUI(true);
            }
            else if (UtilMethods.CollidedWithTroop(other))
            {
                ITroopBuildingInteraction troop = other.GetComponent<Troop>();
                OnTroopInteractWithBuilding(troop);
            }
        }

        protected virtual void OnTroopInteractWithBuilding(ITroopBuildingInteraction troop)
        {

        }


        private void OnTriggerExit2D(Collider2D other)
        {
            if (UtilMethods.CollidedWithPlayer(other))
            {
                _buildingUI.ToggleBuildingUI(false);
            }
        }
    }
}