using System;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
using pvs.settings.debug;
using pvs.utils;
using UnityEngine;
using Zenject;
namespace pvs.logic.playground.building {

	public class BuildingsViewController : MonoBehaviour {

		[Inject] private readonly IPlaygroundBuildingsState playgroundBuildingsState;
		[Inject] private readonly IIsometricInfo isometricInfo;
		[SerializeField] private AudioSource errorAudioSource;

		private Camera playgroundCamera;
		private GameObject underConstructionBuilding;

		private static int counter = 0;

		private void Start() {
			playgroundCamera = Camera.main;
			playgroundBuildingsState.Load(transform);
		}

		private void Update() {
			if (Input.GetKeyUp(KeyCode.R)) {
				VUnityUtils.CleanChildren(transform);
				playgroundBuildingsState.Reset();
				return;
			}
			
			if (Input.GetKeyUp(KeyCode.S)) {
				playgroundBuildingsState.Save();
				return;
			}
			
			if (Input.GetKeyUp(KeyCode.C)) {
				counter++;
				return;
			}

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
				underConstructionBuilding.transform.position = new Vector3(nearest.x, nearest.y, transform.position.z);

				if (nearest == Constants.NULL_VECTOR_2) {
					playgroundBuildingsState.UpdateUnderCursorPoint(null);
					return;
				}

				var nearestGrid = isometricInfo.ConvertToGridPosition(nearest);

				if (Input.GetKeyUp(Constants.FINISH_BUILD_KEY)) {
					FinishBuildingProcess(nearestGrid);
				} else {
					playgroundBuildingsState.UpdateUnderCursorPoint(nearestGrid);
				}
			}
		}

		private Vector2 FindNearestCenterGridPoint() {
			var mousePosition = GetMouseWorldPosition();
			return isometricInfo.GetNearestGridElementCenter(mousePosition) ?? Constants.NULL_VECTOR_2;
		}

		private Vector3 GetMouseWorldPosition() {
			return playgroundCamera.ScreenToWorldPoint(Input.mousePosition);
		}

		private void StartBuildingProcess() {
			var mousePosition = GetMouseWorldPosition();

			var buildingGameObject = playgroundBuildingsState.StartBuildingProcess(GetBuildingType());
			buildingGameObject.transform.parent = transform;
			buildingGameObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

			underConstructionBuilding = buildingGameObject;
		}

		private static BuildingType GetBuildingType() {
			return (counter % 3) switch {
				0 => BuildingType.BARRACKS,
				1 => BuildingType.LARGE_BARRACKS,
				2 => BuildingType.BASHENKA,
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		private void LoadBuilding(BuildingType type, IsometricPoint position) { }

		private void FinishBuildingProcess(IsometricPoint finalBuildingPosition) {
			bool success = playgroundBuildingsState.FinishBuildProcess(finalBuildingPosition);
			if (success) {
				underConstructionBuilding = null;
			} else {
				errorAudioSource.Play();
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