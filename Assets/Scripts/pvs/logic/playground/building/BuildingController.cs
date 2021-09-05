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

		private LineRenderer lineRenderer;
		private bool initialized => buildingState != null;
		private IBuildingState buildingState;

		private void Start() {
			var polygonCollider = gameObject.GetComponent<PolygonCollider2D>();
			lineRenderer = gameObject.GetComponent<LineRenderer>();
			lineRenderer.startWidth = 0.025f;
			lineRenderer.endWidth = 0.025f;
			lineRenderer.useWorldSpace = false;
			lineRenderer.positionCount = polygonCollider.points.Length + 1;

			for (int i = 0; i < polygonCollider.points.Length; i++) {
				lineRenderer.SetPosition(i, new Vector3(polygonCollider.points[i].x, polygonCollider.points[i].y));
			}

			lineRenderer.SetPosition(polygonCollider.points.Length, new Vector3(polygonCollider.points[0].x, polygonCollider.points[0].y));
			lineRenderer.enabled = false;
		}

		public void Init(IBuildingState buildingState) {
			this.buildingState = buildingState;
		}

		public void OnPointerClick(PointerEventData eventData) {
			if (!initialized) return;
			inputRegistry.RegisterCommand(new ParametrizedCommand<IBuildingState>(SELECT_BUILDING, buildingState));
			lineRenderer.enabled = true;
		}

		private void Update() {
			if (!initialized) return;
			HandleSellBuildingCommand();
			TryDisableSelection();
		}
		private void TryDisableSelection() {
			if (inputRegistry.HasCommand(TERRAIN_CLICK)) {
				lineRenderer.enabled = false;
				return;
			}

			var selectBuildingCmd = inputRegistry.GetCommand<ParametrizedCommand<IBuildingState>>(InputCommandType.SELECT_BUILDING);
			if (selectBuildingCmd == null) {
				return;
			}

			if (!Equals(selectBuildingCmd.GetParam().id, buildingState.id)) {
				lineRenderer.enabled = false;
			}
		}

		private void HandleSellBuildingCommand() {
			var sellBuildingCmd = inputRegistry.GetCommand<ParametrizedCommand<IBuildingState>>(SELL_BUILDING);
			if (sellBuildingCmd == null) return;

			if (Equals(sellBuildingCmd.GetParam().Point, buildingState.Point)) {
				playgroundBuildingsState.SellBuilding(buildingState);
				DestroyImmediate(gameObject);
			}
		}
	}
}