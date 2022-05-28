using UnityEngine;


namespace Buildings.BuildableComponent
{
    public class Buildable : BuildingCore
    {
        [SerializeField] private BuildableUI _buildableUI;


        protected override void OnAwake()
        {
            base.OnAwake();
            _buildableUI.Init(new BuildableUI.InitConfig()
            {
                OnClickBuildingCard = ConstructBuilding
            });
        }


        private void ConstructBuilding(BuildingType buildingType)
        {
            GameloopManager.instance.HandleConstructBuilding(new ConstructBuildingAction
            {
                BuildingType = buildingType,
                Cost = 1, //TODO: get cost from building type
                SpawnPoint = transform.position,
            }, StartBuilding);
        }


        private void StartBuilding()
        {
            //NOTE: This is a temporary solution to start building
            //NOTE: Maybe should play building animation
            Destroy(gameObject);
        }
    }
}