using pvs.input;
using pvs.input.command;
using pvs.logic.playground.building.settings;
namespace pvs.ui {
	
	public class BuildingsUIController : AbstractUIController {
		private void Start() {
			RegisterButtonClickCommand("BuildBarracksButton", new ParametrizedCommand<BuildingType>(BuildingType.BARRACKS, true));
			RegisterButtonClickCommand("BuildLargeBarracksButton", new ParametrizedCommand<BuildingType>(BuildingType.LARGE_BARRACKS, true));
			RegisterButtonClickCommand("BuildBashenkaButton", new ParametrizedCommand<BuildingType>(BuildingType.BASHENKA, true));
			RegisterSimpleButtonClickCommand("CancelBuildingProcessButton", InputCommandType.DISABLE_BUILDING_MODE);
		}
	}
}