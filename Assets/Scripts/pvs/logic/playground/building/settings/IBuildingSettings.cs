using System.Collections.Generic;
using pvs.logic.playground.isometric;
using UnityEngine;

namespace pvs.logic.playground.building.settings {

	public interface IBuildingSettings {

		BuildingType buildingType { get; }
		public Object prefab { get; }
		ISet<IsometricPoint> offsetPoints { get; }
	}
}