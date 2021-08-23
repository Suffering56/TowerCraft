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
		private GameObject underConstructionBuilding;
		private float buildingYOffset;

		private static int counter = 0;

		private void Start() {
			playgroundCamera = Camera.main;
			// если хотим чтобы здание было по центру, нужно сместить его пивот на yOffset вниз
			buildingYOffset = -playgroundInitialState.isometricGridHeight / 2;
		}

		private void Update() {
			if (Input.GetKeyUp(Constants.BUILDING_MODE_KEY)) {
				if (!underConstructionBuilding) {
					StartBuildingProcess();
				} else {
					CancelBuildingProcess();
				}
				return;
			}

			if (underConstructionBuilding) {
				var nearest = FindNearestCenterGridPoint();
				underConstructionBuilding.transform.position = new Vector3(nearest.x, nearest.y + buildingYOffset, transform.position.z);

				if (nearest == Constants.NULL_VECTOR_2) {
					playgroundBuildingsState.UpdateUnderCursorPoint(null);
					return;
				}

				var nearestGrid = playgroundInitialState.isometricInfo.ConvertToGridPosition(nearest);

				if (Input.GetKeyUp(Constants.FINISH_BUILD_KEY)) {
					FinishBuildingProcess(nearestGrid);
				} else {
					playgroundBuildingsState.UpdateUnderCursorPoint(nearestGrid);
				}
			}
		}

		private Vector2 FindNearestCenterGridPoint() {
			var mousePosition = GetMouseWorldPosition();
			return playgroundInitialState.isometricInfo.GetNearestGridElementCenter(mousePosition) ?? Constants.NULL_VECTOR_2;
		}

		private Vector3 GetMouseWorldPosition() {
			return playgroundCamera.ScreenToWorldPoint(Input.mousePosition);
		}

		private void StartBuildingProcess() {
			var mousePosition = GetMouseWorldPosition();

			var buildingGameObject = playgroundBuildingsState.StartBuildingProcess(counter++ % 2 == 0 ? BuildingType.BARRACKS : BuildingType.LARGE_BARRACKS);
			buildingGameObject.transform.parent = transform;
			buildingGameObject.transform.position = new Vector3(mousePosition.x, mousePosition.y + buildingYOffset, transform.position.z);

			underConstructionBuilding = buildingGameObject;
		}

		private void FinishBuildingProcess(IsometricGridPosition finalBuildingPosition) {
			bool success = playgroundBuildingsState.FinishBuildProcess(finalBuildingPosition);
			if (success) {
				underConstructionBuilding = null;
			}
		}

		private void CancelBuildingProcess() {
			playgroundBuildingsState.CancelBuildProcess();

			underConstructionBuilding.SetActive(false);
			DestroyImmediate(underConstructionBuilding);

			underConstructionBuilding = null;
		}
	}
}