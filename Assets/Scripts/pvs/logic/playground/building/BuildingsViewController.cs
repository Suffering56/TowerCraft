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
		private float buildingYOffset;

		private void Start() {
			playgroundCamera = Camera.main;
			// если хотим чтобы здание было по центру, нужно сместить его пивот на yOffset вниз
			buildingYOffset = -playgroundInitialState.isometricGridHeight / 2;
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
					bool found = TryFindNearestCenterGridPoint(out var nearest);
					if (!found) return;

					underConstructionBuilding.instanceGameObject.transform.position = new Vector3(nearest.x, nearest.y + buildingYOffset, transform.position.z);
					var nearestGrid = playgroundInitialState.isometricInfo.ConvertToGridPosition(nearest);

					if (!Equals(prevNearestGrid, nearestGrid)) {
						playgroundBuildingsState.SetSelected(prevNearestGrid, false);
						playgroundBuildingsState.SetSelected(nearestGrid, true);
						prevNearestGrid = nearestGrid;
					}
				}
			}
		}

		private bool TryFindNearestCenterGridPoint(out Vector2 successResult) {
			var mousePosition = GetMouseWorldPosition();
			var nearest = playgroundInitialState.isometricInfo.GetNearestGridElementCenter(mousePosition);

			if (nearest.HasValue) {
				successResult = nearest.Value;
				return true;
			}

			successResult = Vector2.positiveInfinity;
			return false;
		}

		private Vector3 GetMouseWorldPosition() {
			return playgroundCamera.ScreenToWorldPoint(Input.mousePosition);
		}

		private void StartBuildingProcess() {
			var mousePosition = GetMouseWorldPosition();

			var building = playgroundBuildingsState.CreateBuilding(BuildingType.BARRACKS);
			building.instanceGameObject.transform.parent = transform;
			building.instanceGameObject.transform.position = new Vector3(mousePosition.x, mousePosition.y + buildingYOffset, transform.position.z);

			underConstructionBuilding = building;
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