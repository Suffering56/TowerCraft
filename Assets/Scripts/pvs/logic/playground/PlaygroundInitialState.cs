using pvs.logic.playground.building;
using pvs.settings.debug;
using pvs.utils.code;
using UnityEngine;
namespace pvs.logic.playground {

	[ZenjectComponent]
	public class PlaygroundInitialState : IPlaygroundInitialState {
		public Rect terrainRect { get; }
		public GameObject terrainElementPrefab { get; }
		public Vector2 isometricElementSize { get; }
		public float bottomUIPanelRelativeHeight => debugSettings.bottomUIPanelRelativeHeight;
		public bool buildingModeEnabled { get; }
		public bool showDebugGrid { get; }

		private readonly DebugSettings debugSettings;

		public PlaygroundInitialState(DebugSettings debugSettings) {
			this.debugSettings = debugSettings;

			terrainRect = debugSettings.terrainRect;
			terrainElementPrefab = debugSettings.terrainElementPrefab;
			isometricElementSize = debugSettings.isometricElementSize;

			buildingModeEnabled = debugSettings.buildingModeEnabled;
			showDebugGrid = debugSettings.showDebugGrid;
		}

		public Color GetIsometricGridColor(GridPointStatus status) {
			return debugSettings.GetIsometricGridColor(status);
		}
	}
}