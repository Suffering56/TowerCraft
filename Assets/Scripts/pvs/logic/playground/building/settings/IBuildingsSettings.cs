namespace pvs.logic.playground.building.settings {
	
	public interface IBuildingsSettings {

		IBuildingSettings GetBuilding(BuildingType type);
	}
}