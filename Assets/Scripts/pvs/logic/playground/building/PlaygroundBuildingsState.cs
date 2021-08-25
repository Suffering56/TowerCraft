using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
		[Inject] private readonly IIsometricInfo isometricInfo;

		// при разработке величина одной ячейки изометрической сетки была (0.5 х 0.25). соответственно если размер ячеек будет изменен, то нужно будет изменить и масштаб строений
		// если строение имеет scale = 1x1, оно будет занимать одну изометрическую ячейку
		private static readonly Vector2 ORIGIN_ISOMETRIC_GRID_SIZE_IN_WORLD_UNITS = new Vector2(0.5f, 0.25f);
		private readonly Vector3 calculatedScale = Vector3.one;

		// если строение занимает более 1й клетки, то здесь будет несколько точек ведущих к одному и тому же стейту
		private readonly IDictionary<IsometricPoint, IBuildingState> buildingsPoints = new Dictionary<IsometricPoint, IBuildingState>();
		private int buildingIdGenerator = 1;

		private IBuildingState underConstructionBuilding;
		private IsometricPoint underCursorPoint;
		public bool buildingModeEnabled => underConstructionBuilding != null;

		public PlaygroundBuildingsState([Inject] IPlaygroundInitialState playgroundInitialState) {
			if (Math.Abs(playgroundInitialState.isometricGridHeight - ORIGIN_ISOMETRIC_GRID_SIZE_IN_WORLD_UNITS.y) > 0.001f) {
				float coefficient = playgroundInitialState.isometricGridHeight / ORIGIN_ISOMETRIC_GRID_SIZE_IN_WORLD_UNITS.y;
				calculatedScale = new Vector3(coefficient, coefficient, 1);
			}
		}

		[NotNull]
		public GameObject StartBuildingProcess(BuildingType type) {
			if (underConstructionBuilding != null) {
				throw new Exception($"building process already started for {underConstructionBuilding}");
			}

			var settings = buildingsSettings.GetBuilding(type);
			var gameObject = container.InstantiatePrefab(settings.prefab);

			gameObject.name = settings.prefab.name;
			gameObject.transform.localScale = calculatedScale;

			underConstructionBuilding = new BuildingState(settings, gameObject);
			return gameObject;
		}

		public bool FinishBuildProcess(IsometricPoint finalBuildingPosition) {
			var allBuildingPositions = Enumerable
			                           .Repeat(finalBuildingPosition, 1)
			                           .Concat(underConstructionBuilding
			                                   .settings
			                                   .offsetPoints
			                                   .Select(offset => finalBuildingPosition + offset)
			                           ).ToList();

			if (allBuildingPositions.Any(point => isometricInfo.IsOutOfGrid(point))) {
				// нельзя строить здание за границами сетки (даже если оно вылезает за них частично)
				return false;
			}

			if (buildingsPoints.Keys.Intersect(allBuildingPositions).Any()) {
				// нельзя строить здание на занятой точке
				return false;
			}

			foreach (var point in allBuildingPositions) {
				buildingsPoints.Add(point, underConstructionBuilding);
			}

			underConstructionBuilding.FinishBuild(buildingIdGenerator++, finalBuildingPosition);
			CancelBuildProcess();
			return true;
		}

		public void CancelBuildProcess() {
			underCursorPoint = null;
			underConstructionBuilding = null;
		}

		public void UpdateUnderCursorPoint(IsometricPoint newUnderCursorGridPoint) {
			underCursorPoint = newUnderCursorGridPoint;
		}

		public GridPointStatus GetGridPointStatus(IsometricPoint checkedPoint) {
			if (underCursorPoint == null || underConstructionBuilding == null) {
				return GridPointStatus.NONE;
			}

			if (Equals(checkedPoint, underCursorPoint) || underConstructionBuilding.settings.offsetPoints.Contains(checkedPoint - underCursorPoint)) {
				return buildingsPoints.ContainsKey(checkedPoint)
					? GridPointStatus.UNAVAILABLE_FOR_BUILD
					: GridPointStatus.AVAILABLE_FOR_BUILD;
			}

			return GridPointStatus.NONE;
		}
	}
}