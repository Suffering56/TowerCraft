using pvs.logic.playground.state;
using pvs.settings.debug;
using pvs.utils;
using UnityEngine;
using Zenject;

namespace pvs.logic.playground {

	public class IsometricGridGeneratorComponent : MonoBehaviour, IDebugSettingsRefreshListener {

		[SerializeField]
		private GameObject gridElementPrefab;

		[Inject] private DiContainer container;
		[Inject] private IPlaygroundInitialState initialState;

		public void OnDebugSettingsRefreshed(DebugSettings debugSettings) {
			initialState ??= debugSettings;
			DrawBuildingModeGrid();
		}

		private void Start() {
			DrawBuildingModeGrid();
		}

		/**
		 * Раситываем scale isometricGridBlockPrefab, исходя из следующей информации:
		 * - размер спрайта префаба = 100х50px, и сам префаб не нарушает этот масштаб
		 * - размер спрайта terrain-а = 100х100px, и его родительский префаб не нарушает этот масштаб
		 */
		private static Vector3 CalculateRelativeBlockScale(float isometricGridYScale) {
			return new Vector3(isometricGridYScale * 2, isometricGridYScale * 2, 1);
		}

		private void DrawBuildingModeGrid() {
			VUnityUtils.CleanChildren(transform);
			if (!initialState.buildingModeEnabled) return;

			float step = initialState.isometricGridHeight / 2; // половина высоты блока

			float maxX = initialState.terrainSize.x / 2 - initialState.isometricGridWidth;
			float minY = -initialState.terrainSize.y / 2 + initialState.isometricGridHeight;

			var blockScale = CalculateRelativeBlockScale(initialState.isometricGridHeight);

			DrawSomeGrid((pivot, row, col) => {
				InstantiateGridBlock(pivot, col, row * 2, blockScale);

				pivot.x += step * 2;
				pivot.y -= step;
				if (pivot.x > maxX || pivot.y < minY) return;

				InstantiateGridBlock(pivot, col, row * 2 + 1, blockScale);
			});
		}
		private void InstantiateGridBlock(Vector2 pos, int xName, int yName, Vector3 scale) {
			var blockInstance = container?.InstantiatePrefab(gridElementPrefab, transform.position, Quaternion.identity, transform)
			                    ?? Instantiate(gridElementPrefab, gameObject.transform, true);

			blockInstance.name = $"{gridElementPrefab}";
			blockInstance.transform.position = new Vector3(pos.x, pos.y, transform.position.z);
			blockInstance.transform.localScale = scale;

			var controller = blockInstance.GetComponent<IsometricGridElementController>();
			controller.Init(xName, yName, initialState);
		}

		private void OnDrawGizmos() {
			if (initialState == null || initialState.showDebugGrid == false) return;

			float z = transform.position.z;
			float step = initialState.isometricGridHeight / 2; // половина высоты блока
			float step2 = step * 2;                            // половина ширины блока/высота блока
			float step4 = step * 4;                            // ширина блока

			DrawSomeGrid((pivot, row, col) => {
				// слева наверх
				Gizmos.DrawLine(new Vector3(pivot.x, pivot.y - step, z), new Vector3(pivot.x + step2, pivot.y, z));
				// слева вниз
				Gizmos.DrawLine(new Vector3(pivot.x, pivot.y - step, z), new Vector3(pivot.x + step2, pivot.y - step2, z));
				// справа наверх
				Gizmos.DrawLine(new Vector3(pivot.x + step4, pivot.y - step, z), new Vector3(pivot.x + step2, pivot.y, z));
				// справа вниз
				Gizmos.DrawLine(new Vector3(pivot.x + step4, pivot.y - step, z), new Vector3(pivot.x + step2, pivot.y - step2, z));
			});
		}

		private delegate void OnDrawGridElement(Vector2 pivot, int row, int col);

		private void DrawSomeGrid(OnDrawGridElement onDrawGridElement) {
			var terrainSize = initialState.terrainSize;
			var width = initialState.isometricGridWidth;
			var height = initialState.isometricGridHeight;

			int gridBlocksCountX = (int)(terrainSize.x / width);
			int gridBlocksCountY = (int)(terrainSize.y / height);

			float xOffset = -terrainSize.x / 2 + (terrainSize.x - gridBlocksCountX * width) / 2;
			float yOffset = terrainSize.y / 2 - (terrainSize.y - gridBlocksCountY * height) / 2;

			for (int i = 0; i < gridBlocksCountY; i++) {
				float pivotY = -(i * height) + yOffset;

				for (int j = 0; j < gridBlocksCountX; j++) {
					float pivotX = j * width + xOffset;
					onDrawGridElement(new Vector2(pivotX, pivotY), i, j);
				}
			}
		}
	}
}