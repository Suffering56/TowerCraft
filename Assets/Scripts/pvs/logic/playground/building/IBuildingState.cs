using System.Collections.Generic;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
using UnityEngine;

namespace pvs.logic.playground.building {

	public interface IBuildingState {
		public int id { get; }                             // идентификатор здания
		public IsometricGridPosition gridPosition { get; } // где построено здание (в какой клетке)
		public IBuildingSettings settings { get; }         // настройки
		public GameObject instanceGameObject { get; }      // ссылка на префаб строения

		public ISet<IsometricGridPosition> moreBusyGridPoints { get; }

		public void FinishBuild(int id, IsometricGridPosition gridPosition);
	}
}