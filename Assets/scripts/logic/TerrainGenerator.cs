using settings;
using settings.support;
using UnityEngine;

namespace logic
{
	public class TerrainGenerator : MonoBehaviour, ISettingsDebugRefreshListener
	{
		private PlaygroundSettings playgroundSettings;

		public void OnSettingsRefreshed(ISettingsHolder settings)
		{
			playgroundSettings = settings.PlaygroundSettings;

			// чистим префаб от ранее сгенерированного ландшафта
			CleanChildren();

			// собственно рисуем ландшафт
			DrawTerrain();
		}

		private void OnDrawGizmos()
		{
			DrawDebugGrid();
		}

		private void DrawDebugGrid()
		{
			if (playgroundSettings == null || playgroundSettings.showGrid == false) return;

			var terrainSize = playgroundSettings.terrainSize;
			float gridBlockWidth = playgroundSettings.gridBlockHeight * 2;

			int gridBlocksCountX = (int)(terrainSize.x / gridBlockWidth);
			int gridBlocksCountY = (int)(terrainSize.y / playgroundSettings.gridBlockHeight);

			float gridTotalWidth = gridBlocksCountX * gridBlockWidth;
			float gridTotalHeight = gridBlocksCountY * playgroundSettings.gridBlockHeight;

			float xOffset = -terrainSize.x / 2 + (terrainSize.x - gridTotalWidth) / 2;
			float yOffset = terrainSize.y / 2 - (terrainSize.y - gridTotalHeight) / 2;

			float step = gridBlockWidth / 2;
			float halfStep = step / 2;

			float z = playgroundSettings.gridZIndex;
			for (int i = 0; i < gridBlocksCountY; i++)
			{
				float y = -(i * playgroundSettings.gridBlockHeight) + yOffset;

				for (int j = 0; j < gridBlocksCountX; j++)
				{
					float x = j * gridBlockWidth + xOffset;
					Gizmos.DrawLine(new Vector3(x + step, y, z), new Vector3(x, y - halfStep, z));
					Gizmos.DrawLine(new Vector3(x + step, y, z), new Vector3(x + step + step, y - halfStep, z));
					Gizmos.DrawLine(new Vector3(x, y - halfStep, z), new Vector3(x + step, y - step, z));
					Gizmos.DrawLine(new Vector3(x + step, y - step, z), new Vector3(x + step + step, y - halfStep, z));
				}
			}
		}

		private void DrawTerrain()
		{

			var terrainSize = playgroundSettings.terrainSize;

			float xOffset = -terrainSize.x / 2;
			float yOffset = terrainSize.y / 2;

			for (int y = 0; y < terrainSize.y; y++)
			{
				for (int x = 0; x < terrainSize.x; x++)
				{
					var blockInstance = Instantiate(playgroundSettings.terrainBlockPrefab, gameObject.transform, true);
					blockInstance.name = $"{playgroundSettings.terrainBlockPrefab.name}[{x},{y}]";
					blockInstance.transform.position = new Vector3(x + xOffset, -y + yOffset, 0);
				}
			}
		}

		private void CleanChildren()
		{
			int childCount = transform.childCount;
			for (int i = childCount - 1; i >= 0; i--)
			{
				DestroyImmediate(transform.GetChild(i).gameObject);
			}
		}
	}
}