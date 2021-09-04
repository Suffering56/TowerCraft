using System.Linq;
using JetBrains.Annotations;
using pvs.input.command;
using pvs.utils.code;
using UnityEngine;
using UnityEngine.UIElements;
using static pvs.input.InputCommandType;

namespace pvs.input {

	[ZenjectComponent]
	public class InputCommandsRegistry : MonoBehaviour {

		// команды разделены на read-write из-за того, что мы this.Update может вызваться после апдейтов других MonoBehaviour-ов
		// а это приведет к тому, что зарегистрированная команда не будет никем обработана и удалится в this.LateUpdate()
		// поэтому при чтении - всегда будем использовать команду из предыдущего фрейма

		private IInputCommand writeCommand; // current update command: сюда просто запоминаем команду с текущего апдейта
		private IInputCommand readCommand;  // prev update command: а с этой непосредственно работаем
		
		 // TODO: если появятся конфликты нужно сделать UI-команды более приоритетными

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

		public bool IsLeftMouseButtonUp() {
			return Input.GetMouseButtonUp((int)MouseButton.LeftMouse);
		}

		public bool IsRightMouseButtonUp() {
			return Input.GetMouseButtonUp((int)MouseButton.RightMouse);
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

		private void Update() {
			if (Input.GetKeyUp(KeyCode.R)) {
				RegisterCommand(new SimpleCommand(RESET_BUILDINGS));
				return;
			}

			if (Input.GetKeyUp(KeyCode.S)) {
				RegisterCommand(new SimpleCommand(SAVE_BUILDINGS));
				return;
			}

			if (Input.GetKeyUp(KeyCode.Escape)) {
				RegisterCommand(new SimpleCommand(DISABLE_BUILDING_MODE));
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