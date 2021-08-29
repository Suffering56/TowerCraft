using pvs.logic.playground.building.settings;
namespace pvs.input.command {
	
	public class SwitchUnderConstructionBuildingTypeCommand: IInputCommand {
		
		private readonly BuildingType buildingType;

		public SwitchUnderConstructionBuildingTypeCommand(BuildingType buildingType) {
			this.buildingType = buildingType;
		}

		public InputCommandType GetCommandType() {
			return InputCommandType.SWITCH_UNDER_CONSTRUCTION_BUILDING_TYPE;
		}

		public BuildingType GetBuildingType() {
			return buildingType;
		}
	}
}