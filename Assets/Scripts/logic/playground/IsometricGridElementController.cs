using settings.debug;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace logic.playground {
	public class IsometricGridElementController : MonoBehaviour, IPointerClickHandler {

		[SerializeField] public int xName;
		[SerializeField] public int yName;
		[Inject] private DebugSettings debugSettings;

		private bool selected = false;

		private void Start() {
			GetSpriteRenderer().color = debugSettings.isometricGridDefaultColor;
		}

		private SpriteRenderer GetSpriteRenderer() {
			return GetComponent<SpriteRenderer>();
		}

		public void OnPointerClick(PointerEventData eventData) {
			selected = !selected;

			GetSpriteRenderer().color = selected
				? debugSettings.isometricGridSelectedColor
				: debugSettings.isometricGridDefaultColor;

			Debug.Log($"OnMouseClick: {name}");
		}
	}
}