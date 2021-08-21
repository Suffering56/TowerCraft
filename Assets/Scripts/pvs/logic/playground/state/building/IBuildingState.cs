using pvs.logic.playground.state.building.settings;
using UnityEngine;
namespace pvs.logic.playground.state.building {
	
	public interface IBuildingState {
		public int id { get; }						// идентификатор здания
		public Vector2 gridPosition { get; }		// где построено здание (в какой клетке)
		public IBuildingSettings settings { get; }	// настройки
		public GameObject viewObject { get; }			// ссылка на префаб строения
		public void FinishBuild(Vector2 gridPosition);
	}
}