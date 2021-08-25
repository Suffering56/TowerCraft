using UnityEngine;
namespace pvs.logic.playground.isometric {

	public class IsometricPoint {
		public int x { get; }
		public int y { get; }

		public IsometricPoint(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public IsometricPoint(Vector2 vector2) {
			this.x = (int)vector2.x;
			this.y = (int)vector2.y;
		}

		public static IsometricPoint operator +(IsometricPoint a, IsometricPoint b) => new IsometricPoint(a.x + b.x, a.y + b.y);

		public static IsometricPoint operator -(IsometricPoint a, IsometricPoint b) => new IsometricPoint(a.x - b.x, a.y - b.y);

		public static IsometricPoint operator *(IsometricPoint a, IsometricPoint b) => new IsometricPoint(a.x * b.x, a.y * b.y);

		public static IsometricPoint operator /(IsometricPoint a, IsometricPoint b) => new IsometricPoint(a.x / b.x, a.y / b.y);

		public static IsometricPoint operator -(IsometricPoint a) => new IsometricPoint(-a.x, -a.y);

		public static IsometricPoint operator *(IsometricPoint a, int d) => new IsometricPoint(a.x * d, a.y * d);

		public static IsometricPoint operator *(int d, IsometricPoint a) => new IsometricPoint(a.x * d, a.y * d);

		public static IsometricPoint operator /(IsometricPoint a, int d) => new IsometricPoint(a.x / d, a.y / d);

		public static Vector2 operator *(IsometricPoint a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);

		public static Vector2 operator *(Vector2 b, IsometricPoint a) => new Vector2(a.x * b.x, a.y * b.y);

		protected bool Equals(IsometricPoint other) {
			return x == other.x && y == other.y;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((IsometricPoint)obj);
		}

		public override int GetHashCode() {
			unchecked {
				return (x * 397) ^ y;
			}
		}
	}
}