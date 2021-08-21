using pvs.logic.playground.state.building.settings;
using UnityEngine;
namespace pvs.logic.playground.state.building {
	
	public class BuildingState : IBuildingState {

		public int id { get; }                            // идентификатор здания
		public Vector2 gridPosition { get; private set; } // где построено здание (в какой клетке)
		public IBuildingSettings settings { get; }        // настройки
		
		public GameObject viewObject { get; }

		public BuildingState(int id, IBuildingSettings settings, GameObject objectLink) {
			this.id = id;
			this.settings = settings;
			this.viewObject = objectLink;
		}

		public void FinishBuild(Vector2 gridPosition) {
			this.gridPosition = gridPosition;
		}
	}
}