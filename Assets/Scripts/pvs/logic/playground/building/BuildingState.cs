using System;
using System.Linq;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
using Unity.VisualScripting;
using UnityEngine;

namespace pvs.logic.playground.building {

	public class BuildingState : IBuildingState {

		private const int UNFINISHED = -1;

		public int id { get; private set; } = UNFINISHED;               // идентификатор здания
		public IsometricGridPosition gridPosition { get; private set; } // где построено здание (в какой клетке)
		public IBuildingSettings settings { get; }                      // настройки
		public GameObject instanceGameObject { get; }

		public BuildingState(IBuildingSettings settings, GameObject objectLink) {
			this.settings = settings;
			instanceGameObject = objectLink;
		}

		public void FinishBuild(int id, IsometricGridPosition gridPosition) {
			if (this.id != UNFINISHED) {
				throw new Exception($"building({this.id}) process with already finished");
			}

			this.id = id;
			this.gridPosition = gridPosition;
			instanceGameObject.name += $"[{id}]";
		}
	}
}