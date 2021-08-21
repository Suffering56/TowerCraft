using System;
using System.Collections.Generic;
using pvs.logic.playground.state.building;
using pvs.logic.playground.state.building.settings;
using pvs.settings.debug;
using UnityEngine;
using Zenject;

namespace pvs.logic.playground.state {

	public class PlaygroundState : IPlaygroundState {

		[Inject] private readonly DiContainer container;
		[Inject] private readonly IBuildingsSettings buildingsSettings;
		public bool buildingModeEnabled { get; }

		// если строение занимает более 1й клетки, то здесь будет несколько точек ведущих к одному и тому же стейту
		private readonly IDictionary<Vector2, IBuildingState> buildingsPoints = new Dictionary<Vector2, IBuildingState>();
		private int buildingIdGenerator = 0;

		public PlaygroundState(DebugSettings debugSettings) {
			buildingModeEnabled = debugSettings.buildingModeEnabled;
		}

		public void FinishBuild(IBuildingState underConstructionBuilding) {
			var gridPosition = new Vector2(15, 15);
			underConstructionBuilding.FinishBuild(gridPosition);
			buildingsPoints.Add(gridPosition, underConstructionBuilding);
		}

		public IBuildingState CreateBuilding(BuildingType type, Vector2 worldPosition, Transform parent) {
			int buildingId = ++buildingIdGenerator;
			var settings = buildingsSettings.GetBuilding(type);
			var gameObject = container.InstantiatePrefab(settings.prefab, worldPosition, Quaternion.identity, parent);

			return new BuildingState(buildingId, settings, gameObject);
		}

		public bool IsBusyGridPoint(Vector2 gridPoint) {
			return buildingsPoints.ContainsKey(gridPoint);
		}
	}
}