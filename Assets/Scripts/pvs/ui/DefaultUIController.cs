using pvs.input;

namespace pvs.ui {
	public class DefaultUIController : AbstractUIController {

		private void Start() {
			RegisterSimpleButtonClickCommand("ShowBuildingsButton", InputCommandType.SHOW_ALL_BUILDINGS_PANEL);
		}
	}
}