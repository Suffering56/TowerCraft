using pvs.logic.playground.building;
using pvs.settings.debug;
using pvs.utils;
using UnityEngine;
using Zenject;

namespace pvs.logic.playground.isometric {

	public class IsometricGridGeneratorComponent : MonoBehaviour, IDebugSettingsRefreshListener {

		[SerializeField]
		private GameObject gridElementPrefab;

		[Inject] private DiContainer container;
		[Inject] private IPlaygroundInitialState initialState;
		private IBuildingState underConstructionBuilding;
		private bool buildingModeEnabled = false;

		public void OnDebugSettingsRefreshed(DebugSettings debugSettings) {
			initialState ??= debugSettings;
			DrawBuildingModeGrid(debugSettings.buildingModeEnabled);
		}

		private void Start() {
			DrawBuildingModeGrid(false);
		}

		private void Update() {
			if (Input.GetKeyUp(Constants.BUILDING_MODE_KEY)) {
				buildingModeEnabled = !buildingModeEnabled;
			} else if (Input.GetKeyUp(Constants.FINISH_BUILD_KEY)) {
				buildingModeEnabled = false;
			} else {
				return;
			}
			DrawBuildingModeGrid(buildingModeEnabled);
		}


		private void DrawBuildingModeGrid(bool enabled) {
			VUnityUtils.CleanChildren(transform);
			if (!enabled) return;

			initialState.isometricInfo.IterateAllElements(InstantiateGridBlock);
		}

		private void InstantiateGridBlock(Vector2 worldPosition, IsometricGridPosition gridPosition, Vector3 scale) {
			var blockInstance = container?.InstantiatePrefab(gridElementPrefab, transform.position, Quaternion.identity, transform)
			                    ?? Instantiate(gridElementPrefab, gameObject.transform, true);

			blockInstance.name = $"{gridElementPrefab.name}";
			blockInstance.transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
			blockInstance.transform.localScale = scale;

			var controller = blockInstance.GetComponent<IsometricGridElementController>();
			controller.Init(gridPosition, initialState);
		}

		private void OnDrawGizmos() {
			if (initialState == null || initialState.showDebugGrid == false) return;

			float z = transform.position.z;
			float step = initialState.isometricGridHeight / 2; // половина высоты блока
			float step2 = step * 2;                            // половина ширины блока/высота блока
			float step4 = step * 4;                            // ширина блока

			initialState.isometricInfo.IterateAllElements((worldPos, gridPos, elementScale) => {
				// слева наверх
				Gizmos.DrawLine(new Vector3(worldPos.x, worldPos.y - step, z), new Vector3(worldPos.x + step2, worldPos.y, z));
				// слева вниз
				Gizmos.DrawLine(new Vector3(worldPos.x, worldPos.y - step, z), new Vector3(worldPos.x + step2, worldPos.y - step2, z));
				// справа наверх
				Gizmos.DrawLine(new Vector3(worldPos.x + step4, worldPos.y - step, z), new Vector3(worldPos.x + step2, worldPos.y, z));
				// справа вниз
				Gizmos.DrawLine(new Vector3(worldPos.x + step4, worldPos.y - step, z), new Vector3(worldPos.x + step2, worldPos.y - step2, z));
			});
		}
	}
}