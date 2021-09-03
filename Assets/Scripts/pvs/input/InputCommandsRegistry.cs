using System.Linq;
using JetBrains.Annotations;
using pvs.input.command;
using pvs.utils.code;
using UnityEngine;
using UnityEngine.UIElements;

namespace pvs.input {

	[ZenjectComponent]
	public class InputCommandsRegistry : MonoBehaviour {

		// команды разделены на read-write из-за того, что мы this.Update может вызваться после апдейтов других MonoBehaviour-ов
		// а это приведет к тому, что зарегистрированная команда не будет никем обработана и удалится в this.LateUpdate()
		// поэтому при чтении - всегда будем использовать команду из предыдущего фрейма

		private IInputCommand writeCommand; // current update command: сюда просто запоминаем команду с текущего апдейта
		private IInputCommand readCommand;  // prev update command: а с этой непосредственно работаем

		public void RegisterSimpleCommand(InputCommandType commandType) {
			RegisterCommand(new SimpleCommand(commandType));
		}

		// ReSharper disable Unity.PerformanceAnalysis
		public void RegisterCommand(IInputCommand command) {
			if (writeCommand != null) {
				Debug.LogError($"cannot register command {command}, because registry already has another command {writeCommand}");
				return;
			}

			writeCommand = command;
		}

		// ReSharper disable Unity.PerformanceAnalysis
		public bool HasCommand(InputCommandType commandType) {
			return readCommand?.GetCommandType() == commandType;
		}

		public bool HasAnyOfCommands(params InputCommandType[] commandTypes) {
			return commandTypes.Any(HasCommand);
		}

		// ReSharper disable Unity.PerformanceAnalysis
		[CanBeNull]
		public T GetCommand<T>(InputCommandType commandType) where T : class, IInputCommand {
			if (readCommand?.GetCommandType() != commandType) {
				return null;
			}

			if (readCommand is T command) {
				return command;
			}

			Debug.LogError($"class cast exception for GetCommand({commandType}). Expected class={typeof(T)}, actual={readCommand.GetType()}");
			return null;
		}

		public bool HasKeyUpCommand(KeyCode code) {
			if (readCommand?.GetCommandType() != InputCommandType.KEY_UP) {
				return false;
			}

			if (readCommand is KeyUpCommand cmd) {
				return cmd.GetKeyCode() == code;
			}

			return false;
		}

		private void Update() {
			if (writeCommand == null || !writeCommand.IsFromUI()) {

				// если команда является IsFromUI, значит игрок кликнул мышкой на кнопку
				// поэтому этот клик мы не будем регистрировать как команду
				// иначе словим "cannot register command {command}, because registry already has another command"
				// ибо клик мышкой и UI-команда прилетают в одном фрейме

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
		}

		private void LateUpdate() {
			// теперь с командой пришедшей к нам в текущем фрейме, можно будет спокойно работать в следующем фрейме
			readCommand = writeCommand;
			writeCommand = null;
		}
	}
}