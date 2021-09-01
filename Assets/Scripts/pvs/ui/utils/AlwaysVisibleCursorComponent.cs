using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace pvs.ui.utils {

	public class AlwaysVisibleCursorComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

		[Inject] private ICursorVisibilityProvider cursorVisibilityProvider;

		public void OnPointerEnter(PointerEventData eventData) {
			Cursor.visible = true;
		}

		public void OnPointerExit(PointerEventData eventData) {
			Cursor.visible = cursorVisibilityProvider.IsCursorVisible();
		}
	}
}