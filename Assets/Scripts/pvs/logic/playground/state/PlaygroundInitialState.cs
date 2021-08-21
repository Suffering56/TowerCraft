using pvs.settings.debug;
using UnityEngine;
namespace pvs.logic.playground.state {

	public class PlaygroundInitialState : IPlaygroundInitialState {
		public Vector2 terrainSize { get; }
		public GameObject terrainElementPrefab { get; }
		public float isometricGridHeight { get; }
		public float isometricGridWidth => isometricGridHeight * 2;
		public bool buildingModeEnabled { get; }
		public Color isometricGridDefaultColor { get; }
		public Color isometricGridSelectedColor { get; }
		public bool showDebugGrid { get; }

		public PlaygroundInitialState(DebugSettings debugSettings) {
			terrainSize = debugSettings.terrainSize;
			terrainElementPrefab = debugSettings.terrainElementPrefab;
			isometricGridHeight = debugSettings.isometricGridHeight;
			buildingModeEnabled = debugSettings.buildingModeEnabled;
			isometricGridDefaultColor = debugSettings.isometricGridDefaultColor;
			isometricGridSelectedColor = debugSettings.isometricGridSelectedColor;
			showDebugGrid = debugSettings.showDebugGrid;
		}
	}
}