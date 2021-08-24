using pvs.logic.playground.building;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace pvs.logic.playground.isometric {

	public class IsometricGridElementController : MonoBehaviour, IPointerClickHandler {

		[Inject] private IPlaygroundInitialState initialState;
		[Inject] private IPlaygroundBuildingsState playgroundBuildingsState;

		private SpriteRenderer spriteRenderer;

		public IsometricGridPosition position { get; private set; }

		private GridPointStatus status = GridPointStatus.NONE;
		private bool isEditor => initialState == null;

		private void Update() {
			var newStatus = playgroundBuildingsState.GetGridPointStatus(position);

			if (status != newStatus) {
				status = newStatus;
				spriteRenderer.color = initialState.GetIsometricGridColor(status);
			}
		}

		// с радостью сделал бы это в Start(), но у нас еще дебаг режим есть
		public void Init(IsometricGridPosition position, IPlaygroundInitialState initialState) {
			this.position = position;

			name += $"[{position.x},{position.y}]";
			if (isEditor) {
				name += ".Debug";
			}

			spriteRenderer = GetComponent<SpriteRenderer>();
			spriteRenderer.color = initialState.GetIsometricGridColor(GridPointStatus.NONE);
			spriteRenderer.sortingOrder = position.y;
		}

		public void OnPointerClick(PointerEventData eventData) {
			Debug.Log($"OnMouseClick: {name}");
		}
	}
}