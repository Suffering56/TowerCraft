using UnityEngine;

namespace pvs.logic.playground.isometric {
	
	public interface IIsometricInfo {
		
		public void IterateAllElements(GridElementConsumer gridElementConsumer);

		/**
		 * @param worldPivotPosition - это левый верхний угол прямоугольника, в который можно обернуть изометрический ромб
		 */
		public delegate void GridElementConsumer(Vector2 worldPivotPosition, IsometricGridPosition gridPosition, Vector3 elementScale);

		public IsometricGridPosition GetNearestGrid(Vector2 mouseWorldPosition);

		public Vector2 GetNearestWorldPoint(Vector2 mouseWorldPosition);
	}
}