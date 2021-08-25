using pvs.logic.playground.building;
using UnityEngine;

namespace pvs.logic.playground {

	public interface IPlaygroundInitialState {

		public Vector2 terrainSize { get; }
		public GameObject terrainElementPrefab { get; }
		public float isometricGridHeight { get; }
		public float isometricGridWidth { get; }
		public Color GetIsometricGridColor(GridPointStatus status);
		public bool showDebugGrid { get; }
	}
}