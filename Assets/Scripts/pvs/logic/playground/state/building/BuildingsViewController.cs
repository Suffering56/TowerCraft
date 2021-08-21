using pvs.logic.playground.state.building.settings;
using pvs.settings.debug;
using UnityEngine;
using Zenject;

namespace pvs.logic.playground.state.building {

	public class BuildingsViewController : MonoBehaviour {

		[Inject] private IPlaygroundState playgroundState;

		private Camera playgroundCamera;
		private IBuildingState underConstructionBuilding;

		private void Start() {
			playgroundCamera = Camera.main;
		}

		private void Update() {
			if (Input.GetKeyUp(Constants.BUILDING_MODE_KEY)) {
				if (underConstructionBuilding == null) {
					StartBuildingProcess(GetMouseWorldPosition());
				} else {
					CancelBuildingProcess();
				}
				return;
			}

			if (underConstructionBuilding != null) {
				if (Input.GetKeyUp(Constants.FINISH_BUILD_KEY)) {
					FinishBuildingProcess();
				} else {
					var mousePosition = GetMouseWorldPosition();
					underConstructionBuilding.instanceGameObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
				}
			}
		}
		private Vector3 GetMouseWorldPosition() {
			return playgroundCamera.ScreenToWorldPoint(Input.mousePosition);
		}

		private void StartBuildingProcess(Vector3 mousePosition) {
			underConstructionBuilding = playgroundState.CreateBuilding(BuildingType.BARRACKS, mousePosition, transform);
		}

		private void FinishBuildingProcess() {
			playgroundState.FinishBuild(underConstructionBuilding);
			underConstructionBuilding = null;
		}

		private void CancelBuildingProcess() {
			var buildingView = underConstructionBuilding.instanceGameObject;
			underConstructionBuilding = null;
			buildingView.SetActive(false);
			DestroyImmediate(buildingView);
		}
	}
}