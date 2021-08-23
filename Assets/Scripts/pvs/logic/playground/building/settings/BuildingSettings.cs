using System.Collections.Generic;
using UnityEngine;
namespace pvs.logic.playground.building.settings {

	public class BuildingSettings : IBuildingSettings {

		private const string PREFIX = "Prefabs/Buildings/";

		public BuildingType buildingType { get; }
		public Object prefab { get; }
		public IReadOnlyList<Vector2> offsetPoints { get; }

		public BuildingSettings(BuildingType buildingType, string buildingPrefabName, IReadOnlyList<Vector2> offsetPoints) {
			this.buildingType = buildingType;
			this.prefab = Resources.Load(PREFIX + buildingPrefabName);
			this.offsetPoints = offsetPoints;
		}
	}
}