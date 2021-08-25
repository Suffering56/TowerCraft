using JetBrains.Annotations;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
using UnityEngine;
namespace pvs.logic.playground.building {

	public interface IPlaygroundBuildingsState {
		
		public bool buildingModeEnabled { get; }

		public GameObject StartBuildingProcess(BuildingType type);

		public bool FinishBuildProcess(IsometricPoint finalBuildingPosition);

		public void CancelBuildProcess();

		public void UpdateUnderCursorPoint([CanBeNull] IsometricPoint newUnderCursorGridPoint);

		GridPointStatus GetGridPointStatus(IsometricPoint checkedPoint);
	}

	public enum GridPointStatus {
		NONE,
		AVAILABLE_FOR_BUILD,
		UNAVAILABLE_FOR_BUILD
	}
}