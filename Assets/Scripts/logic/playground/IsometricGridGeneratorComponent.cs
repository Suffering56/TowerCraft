using settings.debug;
using UnityEngine;
using utils;
using Zenject;

namespace logic.playground {
	public class IsometricGridGeneratorComponent : MonoBehaviour, IDebugSettingsRefreshListener {

		[SerializeField]
		private GameObject gridElementPrefab;

		[Inject] private DiContainer container;
		[Inject] private DebugSettings debugSettings;

		public void OnDebugSettingsRefreshed(DebugSettings debugSettings, bool isForce) {
			if (this.debugSettings == null)
			{
				this.debugSettings = debugSettings;
			}

			if (isForce)
			{
				DrawBuildingModeGrid();
			}
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

			if (!debugSettings.buildingModeEnabled) return;

			float step = debugSettings.isometricGridHeight / 2; // половина высоты блока

			float maxX = debugSettings.terrainSize.x / 2 - debugSettings.isometricGridWidth;
			float minY = -debugSettings.terrainSize.y / 2 + debugSettings.isometricGridHeight;

			var blockScale = CalculateRelativeBlockScale(debugSettings.isometricGridHeight);

			DrawDebugGrid((pivot, row, col) => {
				InstantiateGridBlock(pivot.x, pivot.y, col, row * 2, blockScale);

				pivot.x += step * 2;
				pivot.y -= step;
				if (pivot.x > maxX || pivot.y < minY) return;

				InstantiateGridBlock(pivot.x, pivot.y, col, row * 2 + 1, blockScale);
			});
		}
		private void InstantiateGridBlock(float x, float y, int xName, int yName, Vector3 scale) {
			var blockInstance = container?.InstantiatePrefab(gridElementPrefab, transform.position, Quaternion.identity, transform)
			                    ?? Instantiate(gridElementPrefab, gameObject.transform, true);

			blockInstance.name = $"{gridElementPrefab.name}[{xName},{yName}]";
			if (container == null) blockInstance.name += ".Debug";
			blockInstance.transform.position = new Vector3(x, y, transform.position.z);
			blockInstance.transform.localScale = scale;

			var spriteRenderer = blockInstance.GetComponent<SpriteRenderer>();
			spriteRenderer.color = debugSettings.isometricGridDefaultColor;
			spriteRenderer.sortingOrder = yName;

			var state = blockInstance.GetComponent<IsometricGridElementController>();
			state.xName = xName;
			state.yName = yName;
		}

		private void OnDrawGizmos() {
			if (debugSettings == null || debugSettings.showDebugGrid == false) return;

			float z = transform.position.z;
			float step = debugSettings.isometricGridHeight / 2; // половина высоты блока
			float step2 = step * 2;                             // половина ширины блока/высота блока
			float step4 = step * 4;                             // ширина блока

			DrawDebugGrid((pivot, row, col) => {
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

		private void DrawDebugGrid(OnDrawGridElement onDrawGridElement) {
			var terrainSize = debugSettings.terrainSize;

			int gridBlocksCountX = (int)(terrainSize.x / debugSettings.isometricGridWidth);
			int gridBlocksCountY = (int)(terrainSize.y / debugSettings.isometricGridHeight);

			float xOffset = -terrainSize.x / 2 + (terrainSize.x - gridBlocksCountX * debugSettings.isometricGridWidth) / 2;
			float yOffset = terrainSize.y / 2 - (terrainSize.y - gridBlocksCountY * debugSettings.isometricGridHeight) / 2;

			for (int i = 0; i < gridBlocksCountY; i++)
			{
				float pivotY = -(i * debugSettings.isometricGridHeight) + yOffset;

				for (int j = 0; j < gridBlocksCountX; j++)
				{
					float pivotX = j * debugSettings.isometricGridWidth + xOffset;
					onDrawGridElement(new Vector2(pivotX, pivotY), i, j);
				}
			}
		}
	}
}