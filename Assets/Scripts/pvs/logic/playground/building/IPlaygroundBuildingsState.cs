using JetBrains.Annotations;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.isometric;
using UnityEngine;

namespace pvs.logic.playground.building {

	public interface IPlaygroundBuildingsState {

		void Save();

		void Load(Transform parent);

		void Reset();

		bool buildingModeEnabled { get; }

		GameObject StartBuildingProcess(BuildingType type);

		bool FinishBuildProcess(IsometricPoint finalBuildingPosition);

		void CancelBuildProcess();

		void UpdateUnderCursorPoint([CanBeNull] IsometricPoint newUnderCursorGridPoint);

		GridPointStatus GetGridPointStatus(IsometricPoint checkedPoint);

		IBuildingState GetBuilding([NotNull] IsometricPoint point);

		void SellBuilding([NotNull] IBuildingState buildingState);
	}

	public enum GridPointStatus {
		NONE,
		AVAILABLE_FOR_BUILD,
		UNAVAILABLE_FOR_BUILD
	}
}