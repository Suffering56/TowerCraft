using pvs.logic.playground.state;
using pvs.settings.debug;
using pvs.utils;
using UnityEngine;
using Zenject;
namespace pvs.logic.playground.terrain {
	public class PlaygroundTerrainGeneratorComponent : MonoBehaviour, IDebugSettingsRefreshListener {

		[Inject] private IPlaygroundInitialState initialState;

		public void OnDebugSettingsRefreshed(DebugSettings debugSettings) {
			initialState ??= debugSettings;
			DrawTerrain(true);
		}

		private void Start() {
			DrawTerrain(false);
		}

		private void DrawTerrain(bool isEditor) {
			// чистим префаб от ранее сгенерированного ландшафта
			VUnityUtils.CleanChildren(transform);

			var boxCollider = GetComponent<BoxCollider2D>();
			boxCollider.size = initialState.terrainSize;

			float xOffset = -initialState.terrainSize.x / 2;
			float yOffset = initialState.terrainSize.y / 2;

			for (int y = 0; y < initialState.terrainSize.y; y++) {
				for (int x = 0; x < initialState.terrainSize.x; x++) {
					var blockInstance = Instantiate(initialState.terrainElementPrefab, gameObject.transform, true);
					blockInstance.transform.position = new Vector3(x + xOffset, -y + yOffset, 0);
					blockInstance.name = $"{initialState.terrainElementPrefab.name}[{x},{y}]";
					if (isEditor) blockInstance.name += ".Debug";
				}
			}
		}
	}
}