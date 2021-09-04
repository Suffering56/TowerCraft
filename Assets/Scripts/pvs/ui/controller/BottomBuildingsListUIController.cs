using pvs.input.command;
using pvs.logic.playground.building.settings;
using static pvs.input.InputCommandType;
using static pvs.logic.playground.building.settings.BuildingType;

namespace pvs.ui.controller {

	public class BottomBuildingsListUIController : AbstractUIController {
		private void Start() {
			RegisterButtonClickCommand("BuildBarracksButton", new ParametrizedCommand<BuildingType>(SELECT_BUILDING_TEMPLATE, BARRACKS));
			RegisterButtonClickCommand("BuildLargeBarracksButton", new ParametrizedCommand<BuildingType>(SELECT_BUILDING_TEMPLATE, LARGE_BARRACKS));
			RegisterButtonClickCommand("BuildBashenkaButton", new ParametrizedCommand<BuildingType>(SELECT_BUILDING_TEMPLATE, BASHENKA));
			RegisterSimpleButtonClickCommand("CancelBuildingProcessButton", DISABLE_BUILDING_MODE);
		}
	}
}