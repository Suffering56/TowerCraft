using System;
using pvs.utils;
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

			// внутренний отступ от границы terrain-а до изометрической сетки
			// если размер terrain-а делится на размер элемента сетки без остатка то innerOffset будет нулевым
			var innerOffset = new Vector2(
				(terrainSize.x - maxElementsCount.x * elementSize.x) / 2,
				(terrainSize.y - maxElementsCount.y * elementSize.y) / 2
			);

			zeroWorldPoint = new Vector2(
				-terrainSize.x / 2 + innerOffset.x,
				terrainSize.y / 2 - innerOffset.y
			);
		}

		public IsometricGridPosition ConvertToGridPosition(Vector2 gridCenterWorldPosition) {
			float distanceToFirstGridCenterX = gridCenterWorldPosition.x - (zeroWorldPoint.x + minWorldStep.x);
			float distanceToFirstGridCenterY = (zeroWorldPoint.y - minWorldStep.y) - gridCenterWorldPosition.y;

			int gridX = (int)(distanceToFirstGridCenterX / minWorldStep.x);
			var gridY = (int)(distanceToFirstGridCenterY / minWorldStep.y);

			return new IsometricGridPosition(gridX, gridY);
		}

		public Vector2? GetNearestGridElementCenter(Vector2 mouseWorldPosition) {
			var candidate1 = new Vector2(
				VMath.RoundTo(mouseWorldPosition.x, elementSize.x),
				VMath.RoundTo(mouseWorldPosition.y, elementSize.y)
			);

			var distance1 = candidate1 - mouseWorldPosition;
			var candidate2 = new Vector2(
				candidate1.x - Math.Sign(distance1.x) * minWorldStep.x,
				candidate1.y - Math.Sign(distance1.y) * minWorldStep.y
			);

			var flattenDistance1 = FlattenDistance(distance1);
			var flattenDistance2 = FlattenDistance(candidate2 - mouseWorldPosition);

			var nearest = flattenDistance1.sqrMagnitude <= flattenDistance2.sqrMagnitude
				? candidate1
				: candidate2;

			if (IsOutOfGrid(nearest)) return null;
			return nearest;
		}

		private bool IsOutOfGrid(Vector2 nearest) {
			if (nearest.x < zeroWorldPoint.x + minWorldStep.x) {
				return true;
			}
			if (nearest.x > maxWorldX + minWorldStep.x) {
				return true;
			}
			if (nearest.y > zeroWorldPoint.y - minWorldStep.y) {
				return true;
			}
			if (nearest.y < minWorldY + minWorldStep.x) {
				return true;
			}
			return false;
		}

		/*
		 * для того чтобы каждая точка внутри изометрического ромбика указывала на его центр, а не на соседний
		 * а такое было бы возможно - ибо изометрическая проекция искажает физическое расстояние между объектами
		 * по идее изометрический ромб - это повернутый под определенным углом квадрат
		 * и соответственно расстояние нужно измерять так словно мы имеем дело с квадратами
		 */
		private static Vector2 FlattenDistance(Vector2 distance) {
			return new Vector2(distance.x / 2, distance.y);
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
					gridPosX++;
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
					gridPosX += 2;
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