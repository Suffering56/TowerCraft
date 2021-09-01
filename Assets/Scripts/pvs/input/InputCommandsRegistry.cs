using System.Linq;
using JetBrains.Annotations;
using pvs.input.command;
using pvs.utils.code;
using UnityEngine;
using UnityEngine.UIElements;

namespace pvs.input {

	[ZenjectComponent]
	public class InputCommandsRegistry : MonoBehaviour {

		private IInputCommand currentCommand; // пока нет надобности хранить сразу несколько команд. Если понадобится - можно сделать CommandPack: IInputCommand
		// private readonly KeyCode[] allKeyCodes = Enum.GetValues(typeof(KeyCode)) as KeyCode[];

		public void RegisterSimpleCommand(InputCommandType commandType) {
			RegisterCommand(new SimpleCommand(commandType));
		}

		// ReSharper disable Unity.PerformanceAnalysis
		public void RegisterCommand(IInputCommand command) {
			if (currentCommand != null) {
				Debug.LogError($"cannot register command {command}, because registry already has another command {currentCommand}");
				return;
			}

			currentCommand = command;
		}

		// ReSharper disable Unity.PerformanceAnalysis
		public bool HasCommand(InputCommandType commandType) {
			return currentCommand?.GetCommandType() == commandType;
		}

		public bool HasAnyOfCommands(params InputCommandType[] commandTypes) {
			return commandTypes.Any(HasCommand);
		}

		// ReSharper disable Unity.PerformanceAnalysis
		[CanBeNull]
		public T GetCommand<T>(InputCommandType commandType) where T : class, IInputCommand {
			if (currentCommand?.GetCommandType() != commandType) {
				return null;
			}

			if (currentCommand is T command) {
				return command;
			}

			Debug.LogError($"class cast exception for GetCommand({commandType}). Expected class={typeof(T)}, actual={currentCommand.GetType()}");
			return null;
		}

		public bool HasKeyUpCommand(KeyCode code) {
			if (currentCommand?.GetCommandType() != InputCommandType.KEY_UP) {
				return false;
			}

			if (currentCommand is KeyUpCommand cmd) {
				return cmd.GetKeyCode() == code;
			}

			return false;
		}

		private void Update() {
			if (currentCommand == null || !currentCommand.IsFromUI()) { // чтобы клик мышкой по UI кнопке не создавал LEFT_MOUSE_BUTTON_UP событие в том же фрейме
				if (Input.GetMouseButtonUp((int)MouseButton.LeftMouse)) {
					RegisterSimpleCommand(InputCommandType.LEFT_MOUSE_BUTTON_UP);
					return;
				}
			}

			if (Input.GetMouseButtonUp((int)MouseButton.RightMouse)) {
				RegisterSimpleCommand(InputCommandType.RIGHT_MOUSE_BUTTON_UP);
				return;
			}

			if (Input.GetKeyUp(KeyCode.R)) {
				RegisterSimpleCommand(InputCommandType.RESET_BUILDINGS);
				return;
			}
			
			if (Input.GetKeyUp(KeyCode.S)) {
				RegisterSimpleCommand(InputCommandType.SAVE_BUILDINGS);
				return;
			}
			
			if (Input.GetKeyUp(KeyCode.Escape)) {
				RegisterSimpleCommand(InputCommandType.DISABLE_BUILDING_MODE);
				return;
			}

			// 	// это издевательство, но пока не нашел ничего интереснее
			// 	foreach (var keyCode in allKeyCodes) {
			// 		if (Input.GetKeyUp(keyCode)) {
			// 			RegisterCommand(new KeyUpCommand(keyCode));
			// 			return;
			// 		}
			// 	}
		}

		private void LateUpdate() {
			currentCommand = null;
		}
	}
}