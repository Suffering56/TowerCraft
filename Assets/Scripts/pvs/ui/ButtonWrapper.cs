using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace pvs.ui {

	public class ButtonWrapper {

		private readonly Button button;
		private readonly Image image;
		private readonly Color originColor;

		public ButtonWrapper(GameObject btn) {
			button = btn.GetComponent<Button>();
			image = btn.GetComponent<Image>();
			originColor = image.color;
		}

		public void SetVisible(bool value) {
			button.interactable = value;
			image.color = originColor.WithAlpha(value ? 1 : 0);
		}
	}
}