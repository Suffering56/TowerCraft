using JetBrains.Annotations;
using logic.playground.state;
using settings.debug;
using UnityEngine;
using utils;
using Zenject;

namespace logic.playground {
	public class PlaygroundTerrainGeneratorComponent : MonoBehaviour, IDebugSettingsRefreshListener {

		[Inject] private PlaygroundState playgroundState;

		public void OnDebugSettingsRefreshed(DebugSettings debugSettings, bool isForce) {
			if (isForce)
			{
				DrawTerrain(debugSettings.terrainSize, debugSettings.terrainTexturePrefab);
			}
		}

		private void Start() {
			DrawTerrain(playgroundState.terrainSize, playgroundState.terrainTexturePrefab);
		}

		private void DrawTerrain([NotNull] Vector2 terrainSize, [NotNull] GameObject terrainTexturePrefab) {
			// чистим префаб от ранее сгенерированного ландшафта
			VUnityUtils.CleanChildren(transform);

			var boxCollider = GetComponent<BoxCollider2D>();
			boxCollider.size = terrainSize;

			float xOffset = -terrainSize.x / 2;
			float yOffset = terrainSize.y / 2;

			for (int y = 0; y < terrainSize.y; y++)
			{
				for (int x = 0; x < terrainSize.x; x++)
				{
					var blockInstance = Instantiate(terrainTexturePrefab, gameObject.transform, true);
					blockInstance.name = $"{terrainTexturePrefab.name}[{x},{y}]";
					if (playgroundState == null) blockInstance.name += ".Debug";
					blockInstance.transform.position = new Vector3(x + xOffset, -y + yOffset, 0);
				}
			}
		}
	}
}