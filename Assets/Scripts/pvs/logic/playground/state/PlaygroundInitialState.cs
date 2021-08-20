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




		// [SerializeField]
		// private GameObject sword;
		//
		// private readonly IDictionary<Vector2, BuildingState> busyGridPoints = new Dictionary<Vector2, BuildingState>();
		// private int buildingIdGenerator = 0;
		//
		// public int CreateBuilding(Vector2 gridPoint)
		// {
		// 	// IUnityContainer container = new UnityContainer();
		// 	int buildingId = ++buildingIdGenerator;
		// 	var building = new BuildingState();
		// 	
		// 	busyGridPoints.Add(gridPoint, building);
		// 	
		// 	return buildingId;
		// }
		//
		// public bool IsBusyGridPoint(Vector2 gridPoint)
		// {
		// 	return busyGridPoints.ContainsKey(gridPoint);
		// }
		//
		// private void Awake()
		// {
		// 	throw new NotImplementedException();
		// }
	}
}