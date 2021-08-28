using System;
using pvs.logic.playground.building.settings;
namespace pvs.logic.playground.building.json {

	[Serializable]
	public class BuildingNode {

		public BuildingType buildingType;
		public IsometricPointNode position;
	}
}