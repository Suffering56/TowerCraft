using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
using pvs.settings.debug;
using UnityEngine;
using Zenject;
namespace pvs.logic.playground.building {

	public class BuildingsViewController : MonoBehaviour {

		[Inject] private readonly IPlaygroundInitialState playgroundInitialState;
		[Inject] private readonly IPlaygroundBuildingsState playgroundBuildingsState;

		private Camera playgroundCamera;
		private IBuildingState underConstructionBuilding;
		private IsometricGridPosition prevNearestGrid;

		private void Start() {
			playgroundCamera = Camera.main;
		}

		private void Update() {
			if (Input.GetKeyUp(Constants.BUILDING_MODE_KEY)) {
				if (underConstructionBuilding == null) {
					StartBuildingProcess();
				} else {
					CancelBuildingProcess();
				}
				return;
			}

			if (underConstructionBuilding != null) {
				if (Input.GetKeyUp(Constants.FINISH_BUILD_KEY)) {
					FinishBuildingProcess();
				} else {
					// если хотим чтобы здание было по центру, нужно сместить его пивот на yOffset вниз
					underConstructionBuilding.instanceGameObject.transform.position = CalculateBuildingWorldPosition();
					// var nearestGrid = playgroundInitialState.isometricInfo.GetNearestGrid(playgroundCamera.ScreenToWorldPoint(Input.mousePosition));
					//
					// if (!Equals(prevNearestGrid, nearestGrid)) {
					// 	playgroundBuildingsState.SetSelected(prevNearestGrid, false);
					// 	playgroundBuildingsState.SetSelected(nearestGrid, true);
					// 	prevNearestGrid = nearestGrid;
					// }

					var nearest = playgroundInitialState.isometricInfo.GetNearestWorldPoint(playgroundCamera.ScreenToWorldPoint(Input.mousePosition));
					underConstructionBuilding.instanceGameObject.transform.position = new Vector3(nearest.x, nearest.y, transform.position.z);
				}
			}
		}
		private Vector3 CalculateBuildingWorldPosition() {
			var yOffset = -playgroundInitialState.isometricGridHeight / 2;
			var mousePosition = playgroundCamera.ScreenToWorldPoint(Input.mousePosition);

			return new Vector3(mousePosition.x, mousePosition.y + yOffset, transform.position.z);
		}

		private void StartBuildingProcess() {
			underConstructionBuilding = playgroundBuildingsState.CreateBuilding(BuildingType.BARRACKS);
			underConstructionBuilding.instanceGameObject.transform.parent = transform;
			underConstructionBuilding.instanceGameObject.transform.position = CalculateBuildingWorldPosition();
		}

		private void FinishBuildingProcess() {
			playgroundBuildingsState.FinishBuild(underConstructionBuilding);
			underConstructionBuilding = null;
			prevNearestGrid = null;
		}

		private void CancelBuildingProcess() {
			var buildingView = underConstructionBuilding.instanceGameObject;
			underConstructionBuilding = null;
			buildingView.SetActive(false);
			DestroyImmediate(buildingView);
			prevNearestGrid = null;
		}
	}
}