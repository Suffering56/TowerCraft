using System;
using System.Collections.Generic;
using pvs.logic.playground.state.building.settings;
using pvs.utils.code;
using UnityEngine;
using Zenject;
namespace pvs.logic.playground.state.building {

	[ZenjectComponent]
	public class BuildingsState : IBuildingsState {

		[Inject] private readonly DiContainer container;
		[Inject] private readonly IBuildingsSettings buildingsSettings;

		// при разработке величина одной ячейки изометрической сетки была (0.5 х 0.25). соответственно если размер ячеек будет изменен, то нужно будет изменить и масштаб строений
		private const float EXPECTED_SCALE = 0.25f;
		private readonly Vector3 calculatedScale = Vector3.one;

		// если строение занимает более 1й клетки, то здесь будет несколько точек ведущих к одному и тому же стейту
		private readonly IDictionary<Vector2, IBuildingState> buildingsPoints = new Dictionary<Vector2, IBuildingState>();
		private int buildingIdGenerator = 1;

		public BuildingsState([Inject] IPlaygroundInitialState playgroundInitialState) {
			if (Math.Abs(playgroundInitialState.isometricGridHeight - EXPECTED_SCALE) > 0.001f) {
				float coefficient = playgroundInitialState.isometricGridHeight / EXPECTED_SCALE;
				calculatedScale = new Vector3(coefficient, coefficient, 1);
			}
		}

		public IBuildingState CreateBuilding(BuildingType type, Vector2 worldPosition, Transform parent) {
			var settings = buildingsSettings.GetBuilding(type);
			var gameObject = container.InstantiatePrefab(settings.prefab, worldPosition, Quaternion.identity, parent);
			gameObject.name = settings.prefab.name;
			gameObject.transform.localScale = calculatedScale;

			return new BuildingState(settings, gameObject);
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