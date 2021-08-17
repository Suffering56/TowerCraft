using input;
using settings;
using UnityEngine;

namespace logic.playground
{
	public class IsometricGridElementState : MonoBehaviour, IMouseListener
	{
		[SerializeField]
		public int xName;

		[SerializeField]
		public int yName;

		public PlaygroundSettings playgroundSettings;

		private bool selected = false;

		private void Start()
		{
			var spriteRenderer = GetSpriteRenderer();
			spriteRenderer.color = playgroundSettings.isometricGridDefaultColor;
		}

		private SpriteRenderer GetSpriteRenderer()
		{
			return GetComponentInChildren<SpriteRenderer>();
		}

		public void OnMouseClick(Vector2 mouseWorldPoint)
		{
			selected = !selected;

			GetSpriteRenderer().color = selected
			? playgroundSettings.isometricGridSelectedColor
			: playgroundSettings.isometricGridDefaultColor;

			Debug.Log($"OnMouseClick: {name}");
		}
	}
}