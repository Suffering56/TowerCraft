using UnityEngine;

namespace pvs.logic.playground.isometric {

	public interface IIsometricInfo {
		
		public void IterateAllElements(GridElementConsumer gridElementConsumer);
		
		/**
		 * @param worldPivotPosition - это центр ромбика
		 */
		public delegate void GridElementConsumer(Vector2 worldPosition, IsometricPoint point, Vector3 elementScale);

		public IsometricPoint ConvertToGridPosition(Vector2 mouseWorldPosition);

		public Vector2 ConvertToWorldPosition(IsometricPoint point);

		public Vector2? GetNearestGridElementCenter(Vector2 worldPosition);

		public bool IsOutOfGrid(IsometricPoint point);

		public bool IsOutOfGrid(Vector2 worldPosition, out Direction outDirection);

		Vector3 elementScale { get; }

		int CalculateSortingOrder(float posY);
	}
}