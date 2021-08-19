using JetBrains.Annotations;
using logic.playground.camera.settings;
using settings.debug;
using UnityEngine;
using Zenject;

namespace logic.playground.state {
	public class PlaygroundState {

		[Inject] private readonly TestSettings testSettings;

		public Vector2 terrainSize { get; }

		public GameObject terrainTexturePrefab { get; }

		// public class Provider : SingletonFactory<PlaygroundState>
		// {
		// }













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