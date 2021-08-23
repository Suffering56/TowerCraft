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
		public System.Collections.Generic.ISet<IsometricGridPosition> moreBusyGridPoints { get; private set; }

		public BuildingState(IBuildingSettings settings, GameObject objectLink) {
			id = id;
			this.settings = settings;
			instanceGameObject = objectLink;
		}

		public void FinishBuild(int id, IsometricGridPosition gridPosition) {
			if (this.id == 0) throw new Exception($"building({this.id}) process with already finished");
			this.id = id;
			instanceGameObject.name += $"[{id}]";

			this.gridPosition = gridPosition;
			moreBusyGridPoints = settings
			                     .offsetPoints
			                     .Select(offset => gridPosition + offset)
			                     .ToHashSet();
		}
	}
}