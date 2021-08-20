using pvs.logic.playground.state;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
namespace pvs.logic.playground {

	public class IsometricGridElementController : MonoBehaviour, IPointerClickHandler {

		[Inject] private IPlaygroundInitialState initialState;
		public int xName { get; private set; }
		public int yName { get; private set; }
		
		private bool selected;
		private bool isEditor => initialState == null;

		// с радостью сделал бы это в Start(), но у нас еще дебаг режим есть
		public void Init(int xName, int yName, IPlaygroundInitialState initialState) {
			this.xName = xName;
			this.yName = yName;
			
			name += $"[{xName},{yName}]";
			if (isEditor) {
				name += ".Debug";
			}

			var spriteRenderer = GetSpriteRenderer();
			spriteRenderer.color = initialState.isometricGridDefaultColor;
			spriteRenderer.sortingOrder = yName;
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