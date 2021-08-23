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

		private bool selected;
		private bool isEditor => initialState == null;

		private void Awake() {
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void Update() {
			var isSelected = playgroundBuildingsState.IsSelected(position);
			if (isSelected != selected) {
				selected = isSelected;
				SwitchColor();
			}
		}

		// с радостью сделал бы это в Start(), но у нас еще дебаг режим есть
		public void Init(IsometricGridPosition position, IPlaygroundInitialState initialState) {
			this.position = position;

			name += $"[{position.x},{position.y}]";
			if (isEditor) {
				name += ".Debug";
			}

			spriteRenderer.color = initialState.isometricGridDefaultColor;
			spriteRenderer.sortingOrder = position.y;
		}

		public void OnPointerClick(PointerEventData eventData) {
			selected = !selected;
			SwitchColor();
			Debug.Log($"OnMouseClick: {name}");
		}
		
		private void SwitchColor() {
			spriteRenderer.color = selected
				? initialState.isometricGridSelectedColor
				: initialState.isometricGridDefaultColor;
		}
	}
}