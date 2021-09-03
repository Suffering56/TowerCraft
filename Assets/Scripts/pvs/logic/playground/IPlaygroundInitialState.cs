using pvs.logic.playground.building;
using UnityEngine;

namespace pvs.logic.playground {

	public interface IPlaygroundInitialState {

		public Rect terrainRect { get; }
		public GameObject terrainElementPrefab { get; }
		public Vector2 isometricElementSize { get; }
		public bool showDebugGrid { get; }
		public float bottomUIPanelRelativeHeight { get; }

		public Color GetIsometricGridColor(GridPointStatus status);
	}
}