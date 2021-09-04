using pvs.input;
using UnityEngine;
using Zenject;

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
			if (inputCommandsRegistry.HasCommand(InputCommandType.OPEN_BUILDINGS_LIST_PANEL)) {
				mainPanel.SetActive(false);
				buildingsPanel.SetActive(true);
			}
			
			if (inputCommandsRegistry.HasCommand(InputCommandType.DISABLE_BUILDING_MODE)) {
				mainPanel.SetActive(true);
				buildingsPanel.SetActive(false);
			}
		}
	}
}