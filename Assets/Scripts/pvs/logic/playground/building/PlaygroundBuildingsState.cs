using System;
using System.Collections.Generic;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
using pvs.utils.code;
using UnityEngine;
using Zenject;

namespace pvs.logic.playground.building {

	[ZenjectComponent]
	public class PlaygroundBuildingsState : IPlaygroundBuildingsState {

		[Inject] private readonly DiContainer container;
		[Inject] private readonly IBuildingsSettings buildingsSettings;

		// при разработке величина одной ячейки изометрической сетки была (0.5 х 0.25). соответственно если размер ячеек будет изменен, то нужно будет изменить и масштаб строений
		// если строение имеет scale = 1x1, оно будет занимать одну изометрическую ячейку
		private static readonly Vector2 ORIGIN_ISOMETRIC_GRID_SIZE_IN_WORLD_UNITS = new Vector2(0.5f, 0.25f);
		private readonly Vector3 calculatedScale = Vector3.one;

		// если строение занимает более 1й клетки, то здесь будет несколько точек ведущих к одному и тому же стейту
		private readonly IDictionary<Vector2, IBuildingState> buildingsPoints = new Dictionary<Vector2, IBuildingState>();
		private int buildingIdGenerator = 1;

		private readonly ISet<IsometricGridPosition> selectedGridPoints = new HashSet<IsometricGridPosition>();

		public PlaygroundBuildingsState([Inject] IPlaygroundInitialState playgroundInitialState) {
			if (Math.Abs(playgroundInitialState.isometricGridHeight - ORIGIN_ISOMETRIC_GRID_SIZE_IN_WORLD_UNITS.y) > 0.001f) {
				float coefficient = playgroundInitialState.isometricGridHeight / ORIGIN_ISOMETRIC_GRID_SIZE_IN_WORLD_UNITS.y;
				calculatedScale = new Vector3(coefficient, coefficient, 1);
			}
		}

		public IBuildingState CreateBuilding(BuildingType type) {
			var settings = buildingsSettings.GetBuilding(type);
			var gameObject = container.InstantiatePrefab(settings.prefab);

			gameObject.name = settings.prefab.name;
			gameObject.transform.localScale = calculatedScale;

			return new BuildingState(settings, gameObject);
		}

		public bool IsSelected(IsometricGridPosition position) {
			return selectedGridPoints.Contains(position);
		}

		public void SetSelected(IsometricGridPosition position, bool selected) {
			if (selected) {
				selectedGridPoints.Add(position);
			} else {
				selectedGridPoints.Remove(position);
			}
		}

		public void FinishBuild(IBuildingState underConstructionBuilding) {
			int buildingId = buildingIdGenerator++;
			var gridPosition = new Vector2(buildingId, buildingId); // TODO STUB

			underConstructionBuilding.FinishBuild(buildingId, gridPosition);
			buildingsPoints.Add(gridPosition, underConstructionBuilding);
		}

		public bool IsBusyGridPoint(Vector2 gridPoint) {
			return buildingsPoints.ContainsKey(gridPoint);
		}
	}
}