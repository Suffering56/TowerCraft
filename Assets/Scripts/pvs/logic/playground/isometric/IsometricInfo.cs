using pvs.logic.playground.state;
using pvs.value;
using UnityEngine;

namespace pvs.logic.playground.isometric {

	public class IsometricInfo : IIsometricInfo {

		// минимальный шаг, который нам потребуется для работы с сеткой
		private readonly Vector2 minWorldStep;

		// правый нижний угол сетки в мировых координатах
		private readonly float maxWorldX;
		private readonly float minWorldY;

		// localScale gameObject-а элемента сетки
		private readonly Vector3 elementScale;

		// координаты левого верхнего угла в мировом пространстве (относительно parent-а ессно), в котором будет отрисован первый элемент [0,0]
		// при учете что пивот isometricGridSprite находится в левом верхнем углу (top-left)
		private readonly Vector2 zeroWorldPoint;
		
		// минимальный размер прямоугольника, в который можно обернуть изометрический ромб
		private readonly Vector2 elementSize;

		public IsometricInfo(IPlaygroundInitialState parent) {
			var terrainSize = parent.terrainSize;
			
			elementSize = new Vector2(parent.isometricGridWidth, parent.isometricGridHeight);
			minWorldStep = elementSize / 2;
			elementScale = CalculateElementScale(elementSize.y);

			maxWorldX = terrainSize.x / 2 - elementSize.x;
			minWorldY = -terrainSize.y / 2 + elementSize.y;

			// максимальное количество элементов(по x,y) которое уместится на terrain-e, если обернуть изометрический ромб в кваррат размером isometricGridHeight * isometricGridWidth
			// на самом деле элементов будет: maxElementsCount.x * maxElementsCount.y + (maxElementsCount.x - 1) * (maxElementsCount.y - 1)
			var maxElementsCount = new IsometricGridPosition(
				(int)(terrainSize.x / elementSize.x),
				(int)(terrainSize.y / elementSize.y)
			);

			zeroWorldPoint = new Vector2(
				-terrainSize.x / 2 + (terrainSize.x - maxElementsCount.x * elementSize.x) / 2,
				terrainSize.y / 2 - (terrainSize.y - maxElementsCount.y * elementSize.y) / 2
			);
		}

		public void IterateAllElements(IIsometricInfo.GridElementConsumer gridElementConsumer) {
			if (elementSize.x == 0 || elementSize.y == 0) {
				Debug.LogError($"infinity loop protection: wrong isometric grid element size {elementSize}");
				return;
			}
			
			
			int gridPosY = 0;
			float worldY = zeroWorldPoint.y;

			while (worldY >= minWorldY) {
				int gridPosX = 0;
				float worldX = zeroWorldPoint.x;

				if (gridPosY % 2 == 1) {
					worldX += minWorldStep.x;
				}

				while (worldX <= maxWorldX) {
					gridElementConsumer.Invoke(
						new Vector2(worldX, worldY),
						new IsometricGridPosition(gridPosX, gridPosY),
						elementScale
					);

					worldX += elementSize.x;
					gridPosX++;
				}

				worldY -= minWorldStep.y;
				gridPosY++;
			}
		}

		/**
		 * Раситываем scale isometricGridBlockPrefab, исходя из следующей информации:
		 * - размер спрайта префаба = 100х50px, и сам префаб не нарушает этот масштаб
		 * - размер спрайта terrain-а = 100х100px, и его родительский префаб не нарушает этот масштаб
		 */
		private static Vector3 CalculateElementScale(float isometricGridYScale) {
			return new Vector3(isometricGridYScale * 2, isometricGridYScale * 2, 1);
		}
	}
}