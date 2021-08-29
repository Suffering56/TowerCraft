using System.Collections.Generic;
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

		private IReadOnlyList<GameObject> elementsInstances;
		private bool buildingModeEnabled;

		public void OnDebugSettingsRefreshed(DebugSettings debugSettings) {
			initialState = debugSettings;
			isometricInfo = new IsometricInfo(debugSettings);
			
			InitializeGrid();
			SwitchBuildingGridElementsActivity(debugSettings.buildingModeEnabled);
		}

		private void Start() {
			InitializeGrid();
			SwitchBuildingGridElementsActivity(false);
		}

		private void Update() {
			if (buildingModeEnabled != playgroundBuildingsState.buildingModeEnabled) {
				buildingModeEnabled = playgroundBuildingsState.buildingModeEnabled;
				SwitchBuildingGridElementsActivity(buildingModeEnabled);
			}
		}
		
		private void InitializeGrid() {
			VUnityUtils.CleanChildren(transform);
			elementsInstances = CreateIsometricBuildingModeGrid();
		}

		private IReadOnlyList<GameObject> CreateIsometricBuildingModeGrid() {
			List<GameObject> elements = new List<GameObject>();

			isometricInfo.IterateAllElements((position, point, scale) => {
				var element = InstantiateGridElement(position, point, scale);
				elements.Add(element);
			});

			return elements;
		}
		
		private void SwitchBuildingGridElementsActivity(bool isActive) {
			foreach (var elementsInstance in elementsInstances) {
				elementsInstance.SetActive(isActive);
			}
		}

		private GameObject InstantiateGridElement(Vector2 worldPosition, IsometricPoint point, Vector3 scale) {
			var elementInstance = container?.InstantiatePrefab(gridElementPrefab, transform.position, Quaternion.identity, transform)
			                      ?? Instantiate(gridElementPrefab, gameObject.transform, true);

			elementInstance.name = $"{gridElementPrefab.name}";
			elementInstance.transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
			elementInstance.transform.localScale = scale;

			var controller = elementInstance.GetComponent<IsometricGridElementController>();
			controller.Init(point, initialState);

			return elementInstance;
		}

		private void OnDrawGizmos() {
			if (initialState == null || initialState.showDebugGrid == false) return;

			float z = transform.position.z;
			float stepX = initialState.isometricElementSize.y;
			float stepY = initialState.isometricElementSize.y / 2;

			isometricInfo.IterateAllElements((worldPos, gridPos, elementScale) => {
				var top = new Vector3(worldPos.x, worldPos.y + stepY, z);
				var right = new Vector3(worldPos.x + stepX, worldPos.y, z);
				var bottom = new Vector3(worldPos.x, worldPos.y - stepY, z);
				var left = new Vector3(worldPos.x - stepX, worldPos.y, z);

				Gizmos.DrawLine(left, top);     // слева наверх
				Gizmos.DrawLine(top, right);    // сверху направо
				Gizmos.DrawLine(right, bottom); // справа вниз
				Gizmos.DrawLine(bottom, left);  // снизу налево
			});
		}
	}
}