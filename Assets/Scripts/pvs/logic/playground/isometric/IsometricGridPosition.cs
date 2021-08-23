namespace pvs.logic.playground.isometric {

	public class IsometricGridPosition {
		public int x { get; }
		public int y { get; }

		public IsometricGridPosition(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public static IsometricGridPosition operator +(IsometricGridPosition a, IsometricGridPosition b) => new IsometricGridPosition(a.x + b.x, a.y + b.y);
		public static IsometricGridPosition operator -(IsometricGridPosition a, IsometricGridPosition b) => new IsometricGridPosition(a.x - b.x, a.y - b.y);
		public static IsometricGridPosition operator *(IsometricGridPosition a, IsometricGridPosition b) => new IsometricGridPosition(a.x * b.x, a.y * b.y);
		public static IsometricGridPosition operator /(IsometricGridPosition a, IsometricGridPosition b) => new IsometricGridPosition(a.x / b.x, a.y / b.y);
		public static IsometricGridPosition operator -(IsometricGridPosition a) => new IsometricGridPosition(-a.x, -a.y);
		public static IsometricGridPosition operator *(IsometricGridPosition a, int d) => new IsometricGridPosition(a.x * d, a.y * d);
		public static IsometricGridPosition operator *(int d, IsometricGridPosition a) => new IsometricGridPosition(a.x * d, a.y * d);
		public static IsometricGridPosition operator /(IsometricGridPosition a, int d) => new IsometricGridPosition(a.x / d, a.y / d);


		protected bool Equals(IsometricGridPosition other) {
			return x == other.x && y == other.y;
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((IsometricGridPosition)obj);
		}

		public override int GetHashCode() {
			unchecked {
				return (x * 397) ^ y;
			}
		}
	}
}