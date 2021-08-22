using pvs.logic.playground.isometric;
using UnityEngine;
namespace pvs.logic.playground {

	public interface IPlaygroundInitialState {

		public Vector2 terrainSize { get; }
		public GameObject terrainElementPrefab { get; }
		public float isometricGridHeight { get; }
		public float isometricGridWidth { get; }
		public Color isometricGridDefaultColor { get; }
		public Color isometricGridSelectedColor { get; }
		public bool showDebugGrid { get; }
		public IIsometricInfo isometricInfo { get; }
	}
}