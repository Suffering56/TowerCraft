using pvs.input;
using pvs.input.command;
using pvs.logic.playground.building;

namespace pvs.ui.controller {
	public class BottomMainUIController : AbstractUIController {

		private ButtonWrapper sellBuildingButton;
		private IBuildingState selectedBuilding;

		private void Start() {
			sellBuildingButton = new ButtonWrapper(transform.Find("SellBuildingButton").gameObject);
			sellBuildingButton.SetVisible(false);

			RegisterSimpleButtonClickCommand("ShowBuildingsButton", InputCommandType.OPEN_BUILDINGS_LIST_PANEL);

			RegisterButtonClickCommand("SellBuildingButton", () => selectedBuilding != null
				                           ? new ParametrizedCommand<IBuildingState>(InputCommandType.SELL_BUILDING, selectedBuilding)
				                           : null
			);
		}

		private void Update() {
			var selectBuildingCmd = inputRegistry.GetCommand<ParametrizedCommand<IBuildingState>>(InputCommandType.SELECT_BUILDING);

			if (selectBuildingCmd != null) {
				sellBuildingButton.SetVisible(true);
				selectedBuilding = selectBuildingCmd.GetParam();
				return;
			}

			if (inputRegistry.HasAnyOfCommands(InputCommandType.TERRAIN_CLICK, InputCommandType.SELL_BUILDING)) {
				sellBuildingButton.SetVisible(false);
				selectedBuilding = null;
				return;
			}
		}
	}
}