using JetBrains.Annotations;
using pvs.logic.playground.building.json;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
using UnityEngine;

namespace pvs.logic.playground.building {

	public interface IBuildingState {
		public int id { get; }                             // идентификатор здания
		
		public IsometricPoint Point { get; } // где построено здание (в какой клетке)
		
		public IBuildingSettings settings { get; }         // настройки
		
		public GameObject instanceGameObject { get; }      // ссылка на префаб строения

		public void FinishBuild(int id, IsometricPoint point, IIsometricInfo isometricInfo);

		[NotNull] BuildingNode ToJsonNode();
	}
}