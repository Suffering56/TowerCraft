using pvs.logic.playground.building;
using pvs.settings.debug;
using pvs.utils;
using UnityEngine;
using Zenject;

namespace pvs.logic.playground.isometric {

	public class IsometricGridGeneratorComponent : MonoBehaviour, IDebugSettingsRefreshListener {

		[SerializeField] private GameObject gridElementPrefab;

		[Inject] private DiContainer container;
		[Inject] private IPlaygroundInitialState initialState;
		[Inject] private IPlaygroundBuildingsState playgroundBuildingsState;
		[Inject] private IIsometricInfo isometricInfo;

		private bool buildingModeEnabled;

		public void OnDebugSettingsRefreshed(DebugSettings debugSettings) {
			initialState ??= debugSettings;
			isometricInfo ??= new IsometricInfo(debugSettings);
			
			VUnityUtils.CleanChildren(transform);
			if (debugSettings.buildingModeEnabled) {
				DrawBuildingModeGrid();
			}
		}

		private void Start() {
			VUnityUtils.CleanChildren(transform);
		}

		private void Update() {
			if (buildingModeEnabled != playgroundBuildingsState.buildingModeEnabled) {
				buildingModeEnabled = playgroundBuildingsState.buildingModeEnabled;
				if (buildingModeEnabled) {
					DrawBuildingModeGrid();
				} else {
					VUnityUtils.CleanChildren(transform);
				}
			}
		}

		private void DrawBuildingModeGrid() {
			isometricInfo.IterateAllElements(InstantiateGridBlock);
		}

		private void InstantiateGridBlock(Vector2 worldPosition, IsometricPoint point, Vector3 scale) {
			var blockInstance = container?.InstantiatePrefab(gridElementPrefab, transform.position, Quaternion.identity, transform)
			                    ?? Instantiate(gridElementPrefab, gameObject.transform, true);

			blockInstance.name = $"{gridElementPrefab.name}";
			blockInstance.transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
			blockInstance.transform.localScale = scale;

			var controller = blockInstance.GetComponent<IsometricGridElementController>();
			controller.Init(point, initialState);
		}

		private void OnDrawGizmos() {
			if (initialState == null || initialState.showDebugGrid == false) return;

			float z = transform.position.z;
			float stepX = initialState.isometricGridHeight;
			float stepY = initialState.isometricGridHeight / 2;

			isometricInfo.IterateAllElements((worldPos, gridPos, elementScale) => {
				var top = new Vector3(worldPos.x, worldPos.y + stepY, z);
				var right = new Vector3(worldPos.x + stepX, worldPos.y, z);
				var bottom = new Vector3(worldPos.x, worldPos.y - stepY, z);
				var left = new Vector3(worldPos.x - stepX, worldPos.y, z);

				Gizmos.DrawLine(left, top);		// слева наверх
				Gizmos.DrawLine(top, right);	// сверху направо
				Gizmos.DrawLine(right, bottom);	// справа вниз
				Gizmos.DrawLine(bottom, left);	// снизу налево
			});
		}
	}
}