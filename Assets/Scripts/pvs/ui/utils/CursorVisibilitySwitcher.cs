using pvs.utils.code;
using UnityEngine;
namespace pvs.ui.utils {

	[ZenjectComponent]
	public class CursorVisibilitySwitcher : ICursorVisibilityProvider {

		private bool visible = true;

		public void ShowCursor() {
			SetVisibility(true);
		}

		public void HideCursor() {
			SetVisibility(false);
		}

		public bool IsCursorVisible() {
			return visible;
		}

		private void SetVisibility(bool value) {
			visible = value;
			Cursor.visible = value;
		}
	}
}