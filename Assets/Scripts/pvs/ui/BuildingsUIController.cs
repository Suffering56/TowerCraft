using pvs.input;
using pvs.input.command;
using pvs.logic.playground.building.settings;
namespace pvs.ui {
	
	public class BuildingsUIController : AbstractUIController {
		private void Start() {
			RegisterButtonClickCommand("BuildBarracksButton", new SwitchUnderConstructionBuildingTypeCommand(BuildingType.BARRACKS));
			RegisterButtonClickCommand("BuildLargeBarracksButton", new SwitchUnderConstructionBuildingTypeCommand(BuildingType.LARGE_BARRACKS));
			RegisterButtonClickCommand("BuildBashenkaButton", new SwitchUnderConstructionBuildingTypeCommand(BuildingType.BASHENKA));
			RegisterSimpleButtonClickCommand("CancelBuildingProcessButton", InputCommandType.DISABLE_BUILDING_MODE);
		}
	}
}