using pvs.input;
using UnityEngine;
using Zenject;

namespace pvs.ui {
	public class UiStateSwitcher : MonoBehaviour {

		[Inject] private InputCommandsRegistry inputCommandsRegistry;

		private GameObject defaultPanel;
		private GameObject buildingsPanel;

		private void Start() {
			defaultPanel = transform.Find("DefaultUIPanel").gameObject;
			buildingsPanel = transform.Find("AllBuildingsUIPanel").gameObject;
		}

		private void Update() {
			if (inputCommandsRegistry.HasCommand(InputCommandType.SHOW_ALL_BUILDINGS_PANEL)) {
				defaultPanel.SetActive(false);
				buildingsPanel.SetActive(true);
			}
			
			if (inputCommandsRegistry.HasCommand(InputCommandType.DISABLE_BUILDING_MODE)) {
				defaultPanel.SetActive(true);
				buildingsPanel.SetActive(false);
			}
		}
	}
}