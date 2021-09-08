using System.Collections.Generic;
using pvs.logic.playground.isometric;
using pvs.utils.code;
namespace pvs.logic.playground.building.settings {

	[ZenjectComponent]
	public class BuildingsSettings : IBuildingsSettings {

		private readonly Dictionary<BuildingType, BuildingSettings> buildings;

		private static readonly IsometricPoint[] LARGE_BUILDING_OFFSETS = {
			new IsometricPoint(-1, -1),
			new IsometricPoint(1, -1),
			new IsometricPoint(0, -2),
		};

		public BuildingsSettings() {
			buildings = ReadSettings();
		}

		private Dictionary<BuildingType, BuildingSettings> ReadSettings() {
			return new Dictionary<BuildingType, BuildingSettings> {
				[BuildingType.BARRACKS] = new BuildingSettings(BuildingType.BARRACKS, "Barracks"),
				[BuildingType.LARGE_BARRACKS] = new BuildingSettings(BuildingType.LARGE_BARRACKS, "LargeBarracks", LARGE_BUILDING_OFFSETS),
				[BuildingType.BASHENKA] = new BuildingSettings(BuildingType.BASHENKA, "Bashenka"),
				[BuildingType.FROST_TOWER] = new BuildingSettings(BuildingType.BASHENKA, "FrostTower")
			};
		}

		public IBuildingSettings GetBuilding(BuildingType type) {
			return buildings[type];
		}
	}
}