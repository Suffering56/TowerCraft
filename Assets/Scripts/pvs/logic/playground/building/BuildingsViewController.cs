using pvs.input;
using pvs.input.command;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
using pvs.settings.debug;
using pvs.ui.utils;
using pvs.utils;
using UnityEngine;
using Zenject;
using static pvs.input.InputCommandType;

namespace pvs.logic.playground.building {

	public class BuildingsViewController : MonoBehaviour {

		[Inject] private readonly IPlaygroundBuildingsState playgroundBuildingsState;
		[Inject] private readonly IIsometricInfo isometricInfo;
		[Inject] private readonly InputCommandsRegistry inputRegistry;
		[Inject] private readonly CursorVisibilitySwitcher cursorVisibilitySwitcher;
		[SerializeField] private AudioSource errorAudioSource;

		private Camera playgroundCamera;
		private GameObject underConstructionBuilding;

		private void Start() {
			playgroundCamera = Camera.main;
			playgroundBuildingsState.Load(transform);
		}

		private void Update() {
			if (inputRegistry.HasCommand(RESET_BUILDINGS)) {
				VUnityUtils.CleanChildren(transform);
				playgroundBuildingsState.Reset();
				return;
			}

			if (inputRegistry.HasCommand(SAVE_BUILDINGS)) {
				playgroundBuildingsState.Save();
				return;
			}

			var switchBuildingCommand = inputRegistry.GetCommand<ParametrizedCommand<BuildingType>>(SELECT_BUILDING_TEMPLATE);
			if (switchBuildingCommand != null) {
				if (underConstructionBuilding) {
					CancelBuildingProcess();
				}

				StartBuildingProcess(switchBuildingCommand.GetParam());
				return;
			}

			if (underConstructionBuilding) {
				if (inputRegistry.HasAnyOfCommands(DISABLE_BUILDING_MODE, RIGHT_MOUSE_BUTTON_UP)) {
					CancelBuildingProcess();
					return;
				}
				
				var nearest = FindNearestCenterGridPoint();
				underConstructionBuilding.transform.position = new Vector3(nearest.x, nearest.y, transform.position.z);

				if (nearest == Constants.NULL_VECTOR_2) {
					playgroundBuildingsState.UpdateUnderCursorPoint(null);
					return;
				}

				var nearestGrid = isometricInfo.ConvertToGridPosition(nearest);

				if (inputRegistry.HasCommand(LEFT_MOUSE_BUTTON_UP)) {
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
			cursorVisibilitySwitcher.HideCursor();
		}

		private void FinishBuildingProcess(IsometricPoint finalBuildingPosition) {
			bool success = playgroundBuildingsState.FinishBuildProcess(finalBuildingPosition);
			if (success) {
				underConstructionBuilding = null;
				cursorVisibilitySwitcher.ShowCursor();
			} else {
				errorAudioSource.Play();
			}
		}

		private void CancelBuildingProcess() {
			playgroundBuildingsState.CancelBuildProcess();

			underConstructionBuilding.SetActive(false);
			DestroyImmediate(underConstructionBuilding);

			underConstructionBuilding = null;
			cursorVisibilitySwitcher.ShowCursor();
		}
	}
}