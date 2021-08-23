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

		// при разработке величина одной ячейки изометрической сетки была (0.5 х 0.25). соответственно если размер ячеек будет изменен, то нужно будет изменить и масштаб строений
		// если строение имеет scale = 1x1, оно будет занимать одну изометрическую ячейку
		private static readonly Vector2 ORIGIN_ISOMETRIC_GRID_SIZE_IN_WORLD_UNITS = new Vector2(0.5f, 0.25f);
		private readonly Vector3 calculatedScale = Vector3.one;

		// если строение занимает более 1й клетки, то здесь будет несколько точек ведущих к одному и тому же стейту
		private readonly IDictionary<IsometricGridPosition, IBuildingState> buildingsPoints = new Dictionary<IsometricGridPosition, IBuildingState>();
		private int buildingIdGenerator = 1;

		private IBuildingState underConstructionBuilding;
		private IsometricGridPosition underCursorPoint;

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

		public bool FinishBuildProcess(IsometricGridPosition finalBuildingPosition) {
			// TODO
			
			underConstructionBuilding.FinishBuild(buildingIdGenerator++, finalBuildingPosition);

			buildingsPoints.Add(finalBuildingPosition, underConstructionBuilding);
			foreach (var point in underConstructionBuilding.moreBusyGridPoints) {
				buildingsPoints.Add(point, underConstructionBuilding);
			}
			
			CancelBuildProcess();
			return true;
		}

		public void CancelBuildProcess() {
			underCursorPoint = null;
			underConstructionBuilding = null;
		}

		public void UpdateUnderCursorPoint(IsometricGridPosition newUnderCursorGridPoint) {
			underCursorPoint = newUnderCursorGridPoint;
		}

		public GridPointStatus GetGridPointStatus(IsometricGridPosition checkedPoint) {
			if (underCursorPoint == null || underConstructionBuilding == null) {
				return GridPointStatus.NONE;
			}

			if (Equals(checkedPoint, underCursorPoint)) {
				return buildingsPoints.ContainsKey(checkedPoint)
					? GridPointStatus.UNAVAILABLE_FOR_BUILD
					: GridPointStatus.AVAILABLE_FOR_BUILD;
			}

			var isBusyToo = underConstructionBuilding
			                .settings
			                .offsetPoints
			                .Select(offset => underCursorPoint + offset)
			                .Contains(checkedPoint);

			if (isBusyToo) {
				return buildingsPoints.ContainsKey(checkedPoint)
					? GridPointStatus.UNAVAILABLE_FOR_BUILD
					: GridPointStatus.AVAILABLE_FOR_BUILD;
			}

			return GridPointStatus.NONE;
		}
	}
}