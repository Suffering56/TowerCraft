using System;
using pvs.logic.playground.building.json;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
using UnityEngine;

namespace pvs.logic.playground.building {

	public class BuildingState : IBuildingState {

		private const int UNFINISHED = -1;

		public int id { get; private set; } = UNFINISHED; // идентификатор здания
		public IsometricPoint Point { get; private set; } // где построено здание (в какой клетке)
		public IBuildingSettings settings { get; }        // настройки
		public GameObject instanceGameObject { get; }

		public BuildingState(IBuildingSettings settings, GameObject objectLink) {
			this.settings = settings;
			instanceGameObject = objectLink;
		}

		public void FinishBuild(int id, IsometricPoint point, IIsometricInfo isometricInfo) {
			if (this.id != UNFINISHED) {
				throw new Exception($"building({this.id}) process with already finished");
			}

			this.id = id;
			this.Point = point;
			instanceGameObject.name += $"[{id}]";
			instanceGameObject.GetComponent<SpriteRenderer>().sortingOrder = isometricInfo.CalculateSortingOrder(instanceGameObject.transform.position.y);
			instanceGameObject.GetComponent<BuildingController>().Init(this);
		}

		public BuildingNode ToJsonNode() {
			return new BuildingNode {
				buildingType = settings.buildingType,
				position = Point.ToJsonNode()
			};
		}
	}
}