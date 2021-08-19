using logic.playground.state;
using settings.debug;
using UnityEngine;
using utils;
using Zenject;

namespace logic.playground
{
	public class PlaygroundTerrainGeneratorComponent : MonoBehaviour, IDebugSettingsRefreshListener
	{
		[Inject] private DebugSettings debugSettings;
		private PlaygroundState playgroundState1;
		private PlaygroundState playgroundState2;
		private PlaygroundState playgroundState3;
		
		[Inject]
		private void Construct(PlaygroundState playgroundState) {
			playgroundState1 = playgroundState;
		}
		
		// [Inject]
		// private void Construct(PlaygroundState.Provider playgroundStateProvider, PlaygroundState playgroundState)
		// {
		// 	playgroundState1 = playgroundStateProvider.Get();
		// 	playgroundState2 = playgroundStateProvider.Get();
		// 	playgroundState3 = playgroundStateProvider.Get();
		// }

		public void OnDebugSettingsRefreshed(DebugSettings debugSettings, bool isForce)
		{
			if (isForce)
			{
				DrawTerrain(debugSettings);
			}
		}

		private void Start()
		{
			DrawTerrain(debugSettings);
		}

		private void DrawTerrain(DebugSettings settings)
		{
			// чистим префаб от ранее сгенерированного ландшафта
			VUnityUtils.CleanChildren(transform);

			var terrainSize = settings.terrainSize;

			var boxCollider = GetComponent<BoxCollider2D>();
			boxCollider.size = terrainSize;

			float xOffset = -terrainSize.x / 2;
			float yOffset = terrainSize.y / 2;

			for (int y = 0; y < terrainSize.y; y++)
			{
				for (int x = 0; x < terrainSize.x; x++)
				{
					var blockInstance = Instantiate(settings.terrainTexturePrefab, gameObject.transform, true);
					blockInstance.name = $"{settings.terrainTexturePrefab.name}[{x},{y}]";
					if (playgroundState1 == null) blockInstance.name += ".Debug";
					blockInstance.transform.position = new Vector3(x + xOffset, -y + yOffset, 0);
				}
			}
		}
	}
}