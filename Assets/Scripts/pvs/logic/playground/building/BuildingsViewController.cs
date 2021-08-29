using pvs.input;
using pvs.input.command;
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
		[Inject] private readonly InputCommandsRegistry inputCommandsRegistry;
		[SerializeField] private AudioSource errorAudioSource;

		private Camera playgroundCamera;
		private GameObject underConstructionBuilding;

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

			var switchBuildingCommand = inputCommandsRegistry.GetCommand<SwitchUnderConstructionBuildingTypeCommand>(InputCommandType.SWITCH_UNDER_CONSTRUCTION_BUILDING_TYPE);
			if (switchBuildingCommand != null) {
				if (underConstructionBuilding) {
					CancelBuildingProcess();
				}

				StartBuildingProcess(switchBuildingCommand.GetBuildingType());
				return;
			}

			if (inputCommandsRegistry.HasCommand(InputCommandType.DISABLE_BUILDING_MODE) && underConstructionBuilding) {
				CancelBuildingProcess();
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

				// if (inputRegistry.IsCommandActive(UICommand.FINISH_BUILDING_PROCESS)) {
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

		private void StartBuildingProcess(BuildingType buildingType) {
			var mousePosition = GetMouseWorldPosition();

			var buildingGameObject = playgroundBuildingsState.StartBuildingProcess(buildingType);
			buildingGameObject.transform.parent = transform;
			buildingGameObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

			underConstructionBuilding = buildingGameObject;
		}

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