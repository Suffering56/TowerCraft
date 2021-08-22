using pvs.logic.playground.state.building.settings;
using UnityEngine;

namespace pvs.logic.playground.state.building {
	
	public interface IBuildingsState {
		
		public void FinishBuild(IBuildingState underConstructionBuilding);
		
		public IBuildingState CreateBuilding(BuildingType type, Vector2 worldPosition, Transform parent);
	}
}