using System;
using settings;
using settings.support;
using UnityEngine;
using utils;

namespace logic
{
	public class TerrainGenerator : MonoBehaviour, ISettingsDebugRefreshListener
	{
		private PlaygroundSettings playgroundSettings;

		public void OnSettingsRefreshed(ISettingsHolder settings, bool isForce)
		{
			playgroundSettings = settings.PlaygroundSettings;
			if (isForce)
			{
				DrawTerrain();
			}
		}

		private void Start()
		{
			DrawTerrain();
		}

		private void DrawTerrain()
		{
			// чистим префаб от ранее сгенерированного ландшафта
			VUnityUtils.CleanChildren(transform);

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
	}
}