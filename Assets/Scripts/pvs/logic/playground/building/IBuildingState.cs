using pvs.logic.playground.building.settings;
using UnityEngine;
namespace pvs.logic.playground.building {
	
	public interface IBuildingState {
		public int id { get; }						// идентификатор здания
		public Vector2 gridPosition { get; }		// где построено здание (в какой клетке)
		public IBuildingSettings settings { get; }	// настройки
		public GameObject instanceGameObject { get; }		// ссылка на префаб строения
		public void FinishBuild(int id, Vector2 gridPosition);
	}
}