using System;
using pvs.input;
using pvs.input.command;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace pvs.ui.controller {

	public abstract class AbstractUIController : MonoBehaviour {

		[Inject] protected readonly InputCommandsRegistry inputRegistry;

		protected void RegisterButtonClickCommand(string buttonName, Func<IInputCommand> commandSupplier) {
			transform.Find(buttonName)
			         .GetComponent<Button>()
			         .onClick
			         .AddListener(() => {
				         var command = commandSupplier.Invoke();
				         if (command != null) {
					         inputRegistry.RegisterCommand(command);
				         }
			         });
		}

		protected void RegisterButtonClickCommand(string buttonName, IInputCommand command) {
			RegisterButtonClickCommand(buttonName, () => command);
		}

		protected void RegisterSimpleButtonClickCommand(string buttonName, InputCommandType commandType) {
			RegisterButtonClickCommand(buttonName, new SimpleCommand(commandType));
		}
	}
}