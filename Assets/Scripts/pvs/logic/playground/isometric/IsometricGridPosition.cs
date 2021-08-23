namespace pvs.logic.playground.isometric {

	public class IsometricGridPosition {
		public int x { get; }
		public int y { get; }

		public IsometricGridPosition(int x, int y) {
			this.x = x;
			this.y = y;
		}

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