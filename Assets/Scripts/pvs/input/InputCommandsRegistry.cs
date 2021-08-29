using JetBrains.Annotations;
using pvs.input.command;
using pvs.utils.code;
using UnityEngine;

namespace pvs.input {

	[ZenjectComponent]
	public class InputCommandsRegistry : MonoBehaviour {

		private IInputCommand currentCommand;

		public void RegisterSimpleCommand(InputCommandType commandType) {
			RegisterCommand(new SimpleCommand(commandType));
		}

		public void RegisterCommand(IInputCommand command) {
			if (currentCommand != null) {
				Debug.LogError($"cannot register command {command}, because registry already has another command {currentCommand}");
				return;
			}

			Debug.Log("RegisterCommand");
			currentCommand = command;
		}

		[CanBeNull]
		public T GetCommand<T>(InputCommandType commandType) where T : class, IInputCommand {
			if (currentCommand?.GetCommandType() != commandType) {
				return null;
			}

			Debug.Log($"GotCommand: {commandType}");
			return (T)currentCommand;
		}

		public bool HasCommand(InputCommandType commandType) {
			if (currentCommand?.GetCommandType() != commandType) {
				return false;
			}

			if (currentCommand is SimpleCommand) {
				Debug.LogWarning($"{GetType().Name}.HasCommand called for command {commandType}");
			}

			Debug.Log($"HasCommand: {commandType}");
			return true;
		}

		private void LateUpdate() {
			if (currentCommand != null) {
				Debug.Log("ClearCommand");
				currentCommand = null;
			}
		}
	}
}