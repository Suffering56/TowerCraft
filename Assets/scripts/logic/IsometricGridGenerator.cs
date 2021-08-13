using settings;
using settings.support;
using UnityEngine;
using utils;

namespace logic
{
	public class IsometricGridGenerator : MonoBehaviour, ISettingsDebugRefreshListener
	{
		[SerializeField]
		private GameObject isometricGridBlockPrefab;

		private PlaygroundSettings playgroundSettings;

		public void OnSettingsRefreshed(ISettingsHolder settings, bool isForce)
		{
			playgroundSettings = settings.PlaygroundSettings;
			if (isForce)
			{
				DrawBuildingModeGrid();
			}
		}

		/**
		 * Раситываем scale isometricGridBlockPrefab, исходя из следующей информации:
		 * - размер спрайта префаба = 100х50px, и сам префаб не нарушает этот масштаб
		 * - размер спрайта terrain-а = 100х100px, и его родительский префаб не нарушает этот масштаб
		 */
		private static Vector3 CalculateRelativeBlockScale(float isometricGridYScale)
		{
			return new Vector3(isometricGridYScale * 2, isometricGridYScale * 2, 1);
		}

		private void Start()
		{
			DrawBuildingModeGrid();
		}

		private void DrawBuildingModeGrid()
		{
			VUnityUtils.CleanChildren(transform);

			if (!playgroundSettings.buildingModeEnabled) return;

			float step = playgroundSettings.isometricGridHeight / 2; // половина высоты блока

			float maxX = playgroundSettings.terrainSize.x / 2 - playgroundSettings.isometricGridWidth;
			float minY = -playgroundSettings.terrainSize.y / 2 + playgroundSettings.isometricGridHeight;

			var blockScale = CalculateRelativeBlockScale(playgroundSettings.isometricGridHeight);

			DrawDebugGrid((pivot, row, col) =>
			{
				InstantiateGridBlock(pivot.x, pivot.y, col, row * 2, blockScale);

				pivot.x += step * 2;
				pivot.y -= step;
				if (pivot.x > maxX || pivot.y < minY) return;

				InstantiateGridBlock(pivot.x, pivot.y, col, row * 2 + 1, blockScale);
			});
		}
		private void InstantiateGridBlock(float x, float y, int xName, int yName, Vector3 scale)
		{
			var blockInstance = Instantiate(isometricGridBlockPrefab, gameObject.transform, true);
			blockInstance.name = $"{isometricGridBlockPrefab.name}[{xName},{yName}]";
			blockInstance.transform.position = new Vector3(x, y, transform.position.z);
			blockInstance.transform.localScale = scale;

			var spriteRenderer = blockInstance.GetComponent<SpriteRenderer>();
			spriteRenderer.color = playgroundSettings.isometricGridBlockDefaultColor;
			spriteRenderer.sortingOrder = yName;

			var state = blockInstance.GetComponent<IsometricGridElementState>();
			state.playgroundSettings = playgroundSettings;
			state.xName = xName;
			state.yName = yName;
		}

		private void OnDrawGizmos()
		{
			if (playgroundSettings == null || playgroundSettings.showDebugGrid == false) return;

			float z = transform.position.z;
			float step = playgroundSettings.isometricGridHeight / 2; // половина высоты блока
			float step2 = step * 2;                                  // половина ширины блока/высота блока
			float step4 = step * 4;                                  // ширина блока

			DrawDebugGrid((pivot, row, col) =>
			{
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

		private void DrawDebugGrid(OnDrawGridElement onDrawGridElement)
		{
			var terrainSize = playgroundSettings.terrainSize;

			int gridBlocksCountX = (int)(terrainSize.x / playgroundSettings.isometricGridWidth);
			int gridBlocksCountY = (int)(terrainSize.y / playgroundSettings.isometricGridHeight);

			float xOffset = -terrainSize.x / 2 + (terrainSize.x - gridBlocksCountX * playgroundSettings.isometricGridWidth) / 2;
			float yOffset = terrainSize.y / 2 - (terrainSize.y - gridBlocksCountY * playgroundSettings.isometricGridHeight) / 2;

			for (int i = 0; i < gridBlocksCountY; i++)
			{
				float pivotY = -(i * playgroundSettings.isometricGridHeight) + yOffset;

				for (int j = 0; j < gridBlocksCountX; j++)
				{
					float pivotX = j * playgroundSettings.isometricGridWidth + xOffset;
					onDrawGridElement(new Vector2(pivotX, pivotY), i, j);
				}
			}
		}
	}
}