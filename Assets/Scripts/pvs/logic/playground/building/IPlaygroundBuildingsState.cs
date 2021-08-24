using JetBrains.Annotations;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
using UnityEngine;
namespace pvs.logic.playground.building {

	public interface IPlaygroundBuildingsState {
		public bool buildingModeEnabled { get; }

		public GameObject StartBuildingProcess(BuildingType type);

		public bool FinishBuildProcess(IsometricGridPosition finalBuildingPosition);

		public void CancelBuildProcess();

		public void UpdateUnderCursorPoint([CanBeNull] IsometricGridPosition newUnderCursorGridPoint);

		GridPointStatus GetGridPointStatus(IsometricGridPosition checkedPoint);
	}

	public enum GridPointStatus {
		NONE,
		AVAILABLE_FOR_BUILD,
		UNAVAILABLE_FOR_BUILD
	}
}