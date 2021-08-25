using System;
using pvs.utils;
using pvs.utils.code;
using UnityEngine;

namespace pvs.logic.playground.isometric {

	[ZenjectComponent]
	public class IsometricInfo : IIsometricInfo {

		// минимальный шаг, который нам потребуется для работы с сеткой
		private readonly Vector2 worldStep;

		// localScale gameObject-а элемента сетки
		private readonly Vector3 elementScale;

		// координаты (центра) первого элемента [0,0] в мировом пространстве
		private readonly Vector2 topLeftPoint;

		// координаты (центра) последнего элемента [maxX, maxY]	в мировом пространстве
		private readonly Vector2 bottomRightPoint;

		// минимальный размер прямоугольника, в который можно обернуть изометрический ромб
		private readonly Vector2 elementSize;

		// последний элемент [pivot = bottomRightPoint]
		private readonly IsometricPoint lastPoint;

		public IsometricInfo(IPlaygroundInitialState parent) {
			var terrainSize = parent.terrainSize;

			elementSize = new Vector2(parent.isometricGridWidth, parent.isometricGridHeight);
			worldStep = elementSize / 2;
			elementScale = CalculateElementScale(elementSize.y);

			bottomRightPoint = new Vector2(
				terrainSize.x / 2 - worldStep.x,
				-terrainSize.y / 2 + worldStep.y
			);

			// максимальное количество элементов(по x,y) которое уместится на terrain-e, если обернуть изометрический ромб в кваррат размером isometricGridHeight * isometricGridWidth
			// на самом деле элементов будет: maxElementsCount.x * maxElementsCount.y + (maxElementsCount.x - 1) * (maxElementsCount.y - 1)
			var maxElementsCount = new IsometricPoint(terrainSize / elementSize);

			// внутренний отступ от границы terrain-а до изометрической сетки
			// если размер terrain-а делится на размер элемента сетки без остатка то innerOffset будет нулевым
			var innerOffset = new Vector2(
				(terrainSize.x - maxElementsCount.x * elementSize.x) / 2,
				(terrainSize.y - maxElementsCount.y * elementSize.y) / 2
			);

			topLeftPoint = new Vector2(
				-terrainSize.x / 2 + innerOffset.x + worldStep.x,
				terrainSize.y / 2 - innerOffset.y - worldStep.y
			);

			lastPoint = FindLastPoint();
		}

		private IsometricPoint FindLastPoint() {
			int maxX = 0;
			int maxY = 0;

			IterateAllElements((worldPosition, gridPoint, scale) => {
				maxX = Math.Max(maxX, gridPoint.x);
				maxY = Math.Max(maxY, gridPoint.y);
			});

			return new IsometricPoint(maxX, maxY);
		}

		public IsometricPoint ConvertToGridPosition(Vector2 gridCenterWorldPosition) {
			// расстояние от левого верхнего угла до gridCenterWorldPosition
			float relativeOffsetX = -topLeftPoint.x + gridCenterWorldPosition.x;
			float relativeOffsetY = topLeftPoint.y - gridCenterWorldPosition.y;

			return new IsometricPoint(
				(int)(relativeOffsetX / worldStep.x),
				(int)(relativeOffsetY / worldStep.y)
			);
		}

		public Vector2? GetNearestGridElementCenter(Vector2 mouseWorldPosition) {
			var candidate1 = new Vector2(
				VMath.RoundTo(mouseWorldPosition.x, elementSize.x),
				VMath.RoundTo(mouseWorldPosition.y, elementSize.y)
			);

			var distance1 = candidate1 - mouseWorldPosition;
			var candidate2 = new Vector2(
				candidate1.x - Math.Sign(distance1.x) * worldStep.x,
				candidate1.y - Math.Sign(distance1.y) * worldStep.y
			);

			var flattenDistance1 = FlattenDistance(distance1);
			var flattenDistance2 = FlattenDistance(candidate2 - mouseWorldPosition);

			var nearest = flattenDistance1.sqrMagnitude <= flattenDistance2.sqrMagnitude
				? candidate1
				: candidate2;

			if (IsOutOfGrid(nearest)) return null;
			return nearest;
		}

		public bool IsOutOfGrid(IsometricPoint point) {
			return point.x < 0 ||
			       point.y < 0 ||
			       point.x > lastPoint.x ||
			       point.y > lastPoint.y;
		}

		private bool IsOutOfGrid(Vector2 nearest) {
			if (nearest.x < topLeftPoint.x) {
				return true;
			}
			if (nearest.x > bottomRightPoint.x) {
				return true;
			}
			if (nearest.y > topLeftPoint.y) {
				return true;
			}
			if (nearest.y < bottomRightPoint.y) {
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

		private const int INFINITY_LOOP_PROTECTION = 10000;

		public void IterateAllElements(IIsometricInfo.GridElementConsumer gridElementConsumer) {
			if (elementSize.x == 0 || elementSize.y == 0) {
				Debug.LogError($"infinity loop protection: wrong isometric grid element size {elementSize}");
				return;
			}
			int gridPosY = 0;
			float worldY = topLeftPoint.y;
			int counter = 0;

			while (worldY >= bottomRightPoint.y) {
				int gridPosX = 0;
				float worldX = topLeftPoint.x;

				if (gridPosY % 2 == 1) {
					gridPosX++;
					worldX += worldStep.x;
				}

				while (worldX <= bottomRightPoint.x) {
					// рисуем мать его ромб
					gridElementConsumer.Invoke(
						new Vector2(worldX, worldY),
						new IsometricPoint(gridPosX, gridPosY),
						elementScale
					);

					worldX += elementSize.x;
					gridPosX += 2;

					if (counter++ > INFINITY_LOOP_PROTECTION) {
						Debug.LogError($"{GetType().Name}.IterateAllElements infinity loop protection");
						return;
					}
				}

				worldY -= worldStep.y;
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