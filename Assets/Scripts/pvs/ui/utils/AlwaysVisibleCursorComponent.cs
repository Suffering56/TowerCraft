using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace pvs.ui.utils {

	public class AlwaysVisibleCursorComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ICursorVisibilityChangeListener {

		[Inject] private ICursorVisibilityProvider cursorVisibilityProvider;
		
		private bool componentUnderCursor = false;

		private void Start() {
			cursorVisibilityProvider.RegisterVisibilityChangeListener(this);
		}

		public void OnPointerEnter(PointerEventData eventData) {
			Cursor.visible = true;
			componentUnderCursor = true;
		}

		public void OnPointerExit(PointerEventData eventData) {
			Cursor.visible = cursorVisibilityProvider.IsCursorVisible();
			componentUnderCursor = false;
		}

		public void OnVisibilityChanged(bool actualVisibility) {
			if (componentUnderCursor && actualVisibility == false) {
				Cursor.visible = true;
			}
		}
	}
}