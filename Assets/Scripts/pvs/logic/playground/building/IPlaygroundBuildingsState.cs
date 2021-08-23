using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
namespace pvs.logic.playground.building {

	public interface IPlaygroundBuildingsState {

		public void FinishBuild(IBuildingState underConstructionBuilding);

		public IBuildingState CreateBuilding(BuildingType type);

		bool IsSelected(IsometricGridPosition position);

		public void SetSelected(IsometricGridPosition position, bool selected);
	}
}