using pvs.logic.playground.state;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
namespace pvs.logic.playground.isometric {

	public class IsometricGridElementController : MonoBehaviour, IPointerClickHandler {

		[Inject] private IPlaygroundInitialState initialState;
		public IsometricGridPosition position { get; private set; }

		private bool selected;
		private bool isEditor => initialState == null;

		// с радостью сделал бы это в Start(), но у нас еще дебаг режим есть
		public void Init(IsometricGridPosition position, IPlaygroundInitialState initialState) {
			this.position = position;

			name += $"[{position.x},{position.y}]";
			if (isEditor) {
				name += ".Debug";
			}

			var spriteRenderer = GetSpriteRenderer();
			spriteRenderer.color = initialState.isometricGridDefaultColor;
			spriteRenderer.sortingOrder = position.y;
		}

		private SpriteRenderer GetSpriteRenderer() {
			return GetComponent<SpriteRenderer>();
		}

		public void OnPointerClick(PointerEventData eventData) {
			selected = !selected;

			GetSpriteRenderer().color = selected
				? initialState.isometricGridSelectedColor
				: initialState.isometricGridDefaultColor;

			Debug.Log($"OnMouseClick: {name}");
		}
	}
}