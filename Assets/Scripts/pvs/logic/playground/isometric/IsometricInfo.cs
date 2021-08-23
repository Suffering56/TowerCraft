using System;
using pvs.utils;
using Unity.VisualScripting;
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

		// внутренний отступ от границы terrain-а до изометрической сетки
		// если размер terrain-а делится на размер элемента сетки без остатка то innerOffset будет нулевым
		private readonly Vector2 innerOffset;

		// координаты левого верхнего угла в мировом пространстве (относительно parent-а ессно), в котором будет отрисован первый элемент [0,0]
		// при учете что пивот isometricGridSprite находится в левом верхнем углу (top-left)
		private readonly Vector2 zeroWorldPoint;

		// минимальный размер прямоугольника, в который можно обернуть изометрический ромб
		private readonly Vector2 elementSize;

		private readonly Vector2 gridSize;


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

			innerOffset = new Vector2(
				(terrainSize.x - maxElementsCount.x * elementSize.x) / 2,
				(terrainSize.y - maxElementsCount.y * elementSize.y) / 2
			);

			zeroWorldPoint = new Vector2(
				-terrainSize.x / 2 + innerOffset.x,
				terrainSize.y / 2 - innerOffset.y
			);

			gridSize = new Vector2(
				terrainSize.x - innerOffset.x * 2,
				terrainSize.y - innerOffset.y * 2
			);
		}

		public IsometricGridPosition GetNearestGrid(Vector2 mouseWorldPosition) {
			// -3.8, 3.18
			// float xDiff = mouseWorldPosition.x - (zeroWorldPoint.x + minWorldStep.x);	// 0.95           // 3.8
			// float yDiff = (zeroWorldPoint.y - minWorldStep.y) - mouseWorldPosition.y;	// 1.695          // 13.56   

			// float xDiff = mouseWorldPosition.x - zeroWorldPoint.x; // 1.2
			// float yDiff = zeroWorldPoint.y - mouseWorldPosition.y; // 1.82
			//
			// float xGrid = xDiff / minWorldStep.x;
			// float yGrid = yDiff / minWorldStep.y;
			// bool odd = xGrid % 2 == 1;
			// xGrid /= 2;

			// int yGrid = (int)(yDiff / minWorldStep.y);

			var nearest = GetNearestWorldPoint(mouseWorldPosition);

			var gridX = (int)(nearest.x / minWorldStep.x);
			gridX /= 2;
			var gridY = (int)(nearest.y / minWorldStep.y);

			return new IsometricGridPosition(gridX, gridY);
		}
		public Vector2 GetNearestWorldPoint(Vector2 mouseWorldPosition) {
			var candidate1 = new Vector2(
				VMath.RoundTo(mouseWorldPosition.x, elementSize.x),
				VMath.RoundTo(mouseWorldPosition.y, elementSize.y)
			);

			var distance1 = candidate1 - mouseWorldPosition;
			var candidate2 = new Vector2(
				candidate1.x - Math.Sign(distance1.x) * minWorldStep.x,
				candidate1.y - Math.Sign(distance1.y) * minWorldStep.y
			);

			var distance2 = candidate2 - mouseWorldPosition;

			Vector2 nearest = distance1.sqrMagnitude <= distance2.sqrMagnitude ? candidate1 : candidate2;

			bool isFirstCandidateBetter = distance1.sqrMagnitude <= distance2.sqrMagnitude;

			Debug.DrawLine(
				new Vector3(candidate1.x, candidate1.y, 0),
				new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0),
				isFirstCandidateBetter ? Color.red : Color.white
			);

			Debug.DrawLine(
				new Vector3(candidate2.x, candidate2.y, 0),
				new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0),
				!isFirstCandidateBetter ? Color.red : Color.white
			);

			return new Vector2(nearest.x, nearest.y - minWorldStep.y);
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
					// рисуем мать его ромб
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