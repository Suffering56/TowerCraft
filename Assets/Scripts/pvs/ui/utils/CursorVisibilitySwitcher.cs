using System.Collections.Generic;
using pvs.utils.code;
using UnityEngine;
namespace pvs.ui.utils {

	[ZenjectComponent]
	public class CursorVisibilitySwitcher : ICursorVisibilityProvider {

		private bool visible = true;
		private readonly List<ICursorVisibilityChangeListener> listeners = new List<ICursorVisibilityChangeListener>();

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
			if (this.visible == value) return;

			visible = value;
			Cursor.visible = value;
			listeners.ForEach(listener => listener.OnVisibilityChanged(value));
		}
		
		public void RegisterVisibilityChangeListener(ICursorVisibilityChangeListener listener) {
			listeners.Add(listener);
		}
	}
}