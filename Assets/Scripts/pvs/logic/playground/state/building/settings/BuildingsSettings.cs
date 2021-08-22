using System.Collections.Generic;
using pvs.utils.code;
using UnityEngine;
namespace pvs.logic.playground.state.building.settings {

	[ZenjectComponent]
	public class BuildingsSettings : IBuildingsSettings {

		private readonly Dictionary<BuildingType, BuildingSettings> buildings;

		public BuildingsSettings() {
			buildings = ReadSettings();
		}

		private Dictionary<BuildingType, BuildingSettings> ReadSettings() {
			return new Dictionary<BuildingType, BuildingSettings> {
				[BuildingType.BARRACKS] = new BuildingSettings(BuildingType.BARRACKS, "Barracks", new List<Vector2>()),
				[BuildingType.LARGE_BARRACKS] = new BuildingSettings(BuildingType.LARGE_BARRACKS, "LargeBarracks", new List<Vector2>())
			};
		}

		public IBuildingSettings GetBuilding(BuildingType type) {
			return buildings[type];
		}
	}
}