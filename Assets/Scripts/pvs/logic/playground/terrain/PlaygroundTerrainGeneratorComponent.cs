using pvs.input;
using pvs.input.command;
using pvs.settings.debug;
using pvs.utils;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace pvs.logic.playground.terrain {
	public class PlaygroundTerrainGeneratorComponent : MonoBehaviour, IDebugSettingsRefreshListener, IPointerClickHandler {

		[Inject] private IPlaygroundInitialState initialState;
		[Inject] private InputCommandsRegistry inputRegistry;
		
		[SerializeField] private GameObject backgroundPrefab;
		private Rect terrainRect => initialState.terrainRect;

		public void OnDebugSettingsRefreshed(DebugSettings debugSettings) {
			initialState = debugSettings;
			DrawTerrain(true);
		}

		private void Start() {
			DrawTerrain(false);
		}

		private void DrawTerrain(bool isEditor) {
			// чистим префаб от ранее сгенерированного ландшафта
			VUnityUtils.CleanChildren(transform);
			// DrawBackground(isEditor);

			var boxCollider = GetComponent<BoxCollider2D>();
			boxCollider.size = terrainRect.size;

			for (int y = 0; y < terrainRect.size.y; y++) {
				for (int x = 0; x < terrainRect.size.x; x++) {
					var blockInstance = Instantiate(initialState.terrainElementPrefab, gameObject.transform, true);
					blockInstance.transform.position = new Vector3(x + terrainRect.position.x, -y + terrainRect.position.y, 0);
					blockInstance.name = $"{initialState.terrainElementPrefab.name}[{x},{y}]";
					if (isEditor) blockInstance.name += ".Debug";
				}
			}
		}

		private void DrawBackground(bool isEditor) {
			var blockInstance = Instantiate(backgroundPrefab, gameObject.transform, true);
			blockInstance.transform.localScale = (terrainRect.size * 1.1f).ToVector3(1);
			blockInstance.transform.position = Vector3.zero;

			blockInstance.name = $"{backgroundPrefab.name}";
			if (isEditor) blockInstance.name += ".Debug";
		}

		public void OnPointerClick(PointerEventData eventData) {
			inputRegistry.RegisterCommand(new SimpleCommand(InputCommandType.TERRAIN_CLICK));
		}
	}
}