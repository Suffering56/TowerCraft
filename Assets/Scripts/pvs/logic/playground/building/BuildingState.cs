using System;
using pvs.logic.playground.building.settings;
using UnityEngine;

namespace pvs.logic.playground.building {

	public class BuildingState : IBuildingState {

		private const int UNFINISHED = -1;

		public int id { get; private set; } = UNFINISHED; // идентификатор здания
		public Vector2 gridPosition { get; private set; } // где построено здание (в какой клетке)
		public IBuildingSettings settings { get; }        // настройки
		public GameObject instanceGameObject { get; }

		public BuildingState(IBuildingSettings settings, GameObject objectLink) {
			id = id;
			this.settings = settings;
			instanceGameObject = objectLink;
		}

		public void FinishBuild(int id, Vector2 gridPosition) {
			if (this.id == 0) throw new Exception($"building({this.id}) process with already finished");
			this.id = id;
			this.gridPosition = gridPosition;
			instanceGameObject.name += $"[{id}]";
		}
	}
}