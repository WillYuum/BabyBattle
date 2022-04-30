using GameplayUtils.Methods;
using UnityEngine;


namespace Buildings
{
    public enum FriendOrFoe { None, Friend, Foe };
    public enum BuildingType { Destroyed, TroopCamp, DefensiveWall };

    public class BuildingCore : MonoBehaviour
    {
        protected FriendOrFoe _friendOrFoe;
        [SerializeField] protected BuildingType _buildingType;

        [SerializeField] protected BuildingUI _buildingUI;


        // private List<Troop> _troops = new List<Troop>();


        void Awake()
        {
            OnAwake();

        }

        protected virtual void OnAwake()
        {

        }


        public void Init(ConstructBuildingAction constructBuildingAction)
        {
            _friendOrFoe = constructBuildingAction.FriendOrFoe;

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