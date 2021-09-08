using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using pvs.logic.playground.building.json;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
using pvs.utils;
using pvs.utils.code;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace pvs.logic.playground.building {

	[ZenjectComponent]
	public class PlaygroundBuildingsState : IPlaygroundBuildingsState {

		[Inject] private readonly DiContainer container;
		[Inject] private readonly IBuildingsSettings buildingsSettings;
		[Inject] private readonly IIsometricInfo isometricInfo;

		private int buildingIdGenerator = 1;
		private const string JSON_FILE_NAME = "playgroundState.json";
		public bool buildingModeEnabled => underConstructionBuilding != null;

		// если строение занимает более 1й клетки, то здесь будет несколько точек ведущих к одному и тому же стейту
		private readonly IDictionary<IsometricPoint, IBuildingState> buildingsPoints = new Dictionary<IsometricPoint, IBuildingState>();
		private IBuildingState underConstructionBuilding;
		private IsometricPoint underCursorPoint;

		public void Reset() {
			buildingsPoints.Clear();
		}

		public void Save() {
			var playgroundBuildingNode = new PlaygroundBuildingsStateNode {
				buildings = buildingsPoints
				            .Values
				            .DistinctBy(building => building.id)
				            .Select(building => building.ToJsonNode())
				            .ToArray()
			};

			JsonUtils.WriteJson(JSON_FILE_NAME, playgroundBuildingNode);
		}

		public void Load(Transform parent) {
			var loadedState = JsonUtils.ReadJson<PlaygroundBuildingsStateNode>(JSON_FILE_NAME);
			if (loadedState == default(PlaygroundBuildingsStateNode)) {
				return;
			}

			foreach (var buildingNode in loadedState.buildings) {
				var gameObject = StartBuildingProcess(buildingNode.buildingType);
				var gridPosition = buildingNode.position.ToPoint();

				gameObject.transform.parent = parent;
				gameObject.transform.position = isometricInfo
				                                .ConvertToWorldPosition(gridPosition)
				                                .ToVector3(parent.position.z);

				FinishBuildProcess(gridPosition);
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
			gameObject.transform.localScale = isometricInfo.elementScale;
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = short.MaxValue;

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

			underConstructionBuilding.FinishBuild(buildingIdGenerator++, finalBuildingPosition, isometricInfo);
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

		[CanBeNull]
		public IBuildingState GetBuilding(IsometricPoint point) {
			return buildingsPoints.TryGetValue(point, out var building)
				? building
				: null;
		}

		public void SellBuilding(IBuildingState buildingState) { }
	}
}