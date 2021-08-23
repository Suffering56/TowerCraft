using System.Collections.Generic;
using UnityEngine;
namespace pvs.logic.playground.building.settings {

	public interface IBuildingSettings {

		BuildingType buildingType { get; }
		public Object prefab { get; }
		IReadOnlyList<Vector2> offsetPoints { get; }
	}
}