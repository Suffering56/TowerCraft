using System;
using pvs.input;
using pvs.input.command;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using static pvs.input.InputCommandType;

namespace pvs.logic.playground.building {

	public class BuildingController : MonoBehaviour, IPointerClickHandler {

		[Inject] private InputCommandsRegistry inputRegistry;
		[Inject] private IPlaygroundBuildingsState playgroundBuildingsState;

		private bool initialized => buildingState != null;
		private IBuildingState buildingState;

		public void Init(IBuildingState buildingState) {
			this.buildingState = buildingState;
		}

		public void OnPointerClick(PointerEventData eventData) {
			if (!initialized) return;
			inputRegistry.RegisterCommand(new ParametrizedCommand<IBuildingState>(SELECT_BUILDING, buildingState));
		}

		private void Update() {
			if (!initialized) return;

			var sellBuildingCmd = inputRegistry.GetCommand<ParametrizedCommand<IBuildingState>>(SELL_BUILDING);
			if (sellBuildingCmd == null) return;

			if (Equals(sellBuildingCmd.GetParam().Point, buildingState.Point)) {
				playgroundBuildingsState.SellBuilding(buildingState);
				DestroyImmediate(gameObject);
			}
		}

	}
}