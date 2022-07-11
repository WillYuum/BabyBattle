using GameplayUtils.Classes;
using UnityEngine;


namespace Buildings
{
    public enum BuildingType { Buildable, TroopCamp, DefensiveWall, MainCamp };

    public class BuildingCore : MonoBehaviour
    {
        public FriendOrFoe FriendOrFoe { get; private set; }
        [field: SerializeField] protected BuildingType BuildingType { get; private set; }
        protected HP _hp;

        void Awake()
        {
            _hp = new HP(100);
            OnAwake();
        }

        protected virtual void OnAwake()
        {

        }

        public virtual void DestroyBuilding()
        {
            Destroy(gameObject, 2.0f);
        }
    }
}