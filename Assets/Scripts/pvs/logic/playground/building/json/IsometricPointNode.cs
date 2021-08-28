using System;
using pvs.logic.playground.isometric;
namespace pvs.logic.playground.building.json {

	[Serializable]
	public class IsometricPointNode {
		public int x;
		public int y;

		public IsometricPoint ToPoint() {
			return new IsometricPoint(x, y);
		}
	}
}