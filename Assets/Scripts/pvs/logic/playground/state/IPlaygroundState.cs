using pvs.logic.playground.state.building;
using pvs.logic.playground.state.building.settings;
using UnityEngine;
namespace pvs.logic.playground.state {
	
	public interface IPlaygroundState {
		
		public bool buildingModeEnabled { get; }

		public void FinishBuild(IBuildingState underConstructionBuilding);
		
		public IBuildingState CreateBuilding(BuildingType type, Vector2 worldPosition, Transform parent);
	}
}