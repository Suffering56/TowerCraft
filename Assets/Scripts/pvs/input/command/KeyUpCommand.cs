using UnityEngine;
namespace pvs.input.command {

	public class KeyUpCommand : IInputCommand {

		private readonly KeyCode keyCode;

		public KeyUpCommand(KeyCode keyCode) {
			this.keyCode = keyCode;
		}

		public InputCommandType GetCommandType() {
			return InputCommandType.KEY_UP;
		}

		public KeyCode GetKeyCode() {
			return keyCode;
		}
		
		public bool IsFromUI() {
			return false;
		}
	}
}