using pvs.logic.playground.building;
using pvs.settings.debug;
using pvs.utils.code;
using UnityEngine;
namespace pvs.logic.playground {

	[ZenjectComponent]
	public class PlaygroundInitialState : IPlaygroundInitialState {
		public Vector2 terrainSize { get; }
		public GameObject terrainElementPrefab { get; }
		public float isometricGridHeight { get; }
		public float isometricGridWidth => isometricGridHeight * 2;
		public bool buildingModeEnabled { get; }
		public bool showDebugGrid { get; }

		private readonly DebugSettings debugSettings;

		public PlaygroundInitialState(DebugSettings debugSettings) {
			this.debugSettings = debugSettings;
			
			terrainSize = debugSettings.terrainSize;
			terrainElementPrefab = debugSettings.terrainElementPrefab;
			isometricGridHeight = debugSettings.isometricGridHeight;

			buildingModeEnabled = debugSettings.buildingModeEnabled;
			showDebugGrid = debugSettings.showDebugGrid;
		}

		public Color GetIsometricGridColor(GridPointStatus status) {
			return debugSettings.GetIsometricGridColor(status);
		}
	}
}