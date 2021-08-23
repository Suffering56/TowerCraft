using System.Collections.Generic;
using pvs.logic.playground.isometric;
using UnityEngine;
namespace pvs.logic.playground.building.settings {

	public class BuildingSettings : IBuildingSettings {

		private const string PREFIX = "Prefabs/Buildings/";

		public BuildingType buildingType { get; }
		public Object prefab { get; }
		public IReadOnlyList<IsometricGridPosition> offsetPoints { get; }

		public BuildingSettings(BuildingType buildingType, string buildingPrefabName, params IsometricGridPosition[] offsetPoints) {
			this.buildingType = buildingType;
			prefab = Resources.Load(PREFIX + buildingPrefabName);
			this.offsetPoints = offsetPoints;
		}
	}
}