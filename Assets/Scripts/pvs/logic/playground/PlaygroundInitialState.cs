using pvs.logic.playground.building;
using pvs.logic.playground.isometric;
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

		public IIsometricInfo isometricInfo { get; }

		private readonly DebugSettings debugSettings;

		public PlaygroundInitialState(DebugSettings debugSettings) {
			this.debugSettings = debugSettings;
			
			terrainSize = debugSettings.terrainSize;
			terrainElementPrefab = debugSettings.terrainElementPrefab;
			isometricGridHeight = debugSettings.isometricGridHeight;

			buildingModeEnabled = debugSettings.buildingModeEnabled;
			showDebugGrid = debugSettings.showDebugGrid;

			isometricInfo = new IsometricInfo(this);
		}

		public Color GetIsometricGridColor(GridPointStatus status) {
			return debugSettings.GetIsometricGridColor(status);
		}
	}
}