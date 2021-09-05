using pvs.input;
using UnityEngine;
using Zenject;
using static pvs.input.InputCommandType;

namespace pvs.ui {
	public class UiStateSwitcher : MonoBehaviour {

		[Inject] private InputCommandsRegistry inputCommandsRegistry;

		private GameObject mainPanel;
		private GameObject buildingsPanel;

		private void Start() {
			mainPanel = transform.Find("BottomMainUIPanel").gameObject;
			buildingsPanel = transform.Find("BottomBuildingsListUIPanel").gameObject;
		}

		private void Update() {
			if (inputCommandsRegistry.HasCommand(OPEN_BUILDINGS_LIST_PANEL)) {
				mainPanel.SetActive(false);
				buildingsPanel.SetActive(true);
			}
			
			if (inputCommandsRegistry.HasAnyOfCommands(DISABLE_BUILDING_MODE, SELECT_BUILDING)) {
				mainPanel.SetActive(true);
				buildingsPanel.SetActive(false);
			}
		}
	}
}