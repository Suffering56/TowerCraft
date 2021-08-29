using pvs.input;
using pvs.input.command;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace pvs.ui {

	public abstract class AbstractUIController : MonoBehaviour {

		[Inject] protected readonly InputCommandsRegistry inputCommandsRegistry;
		
		protected void RegisterButtonClickCommand(string buttonName, IInputCommand command) {
			transform.Find(buttonName)
			         .GetComponent<Button>()
			         .onClick
			         .AddListener(() => { inputCommandsRegistry.RegisterCommand(command); });
		}

		protected void RegisterSimpleButtonClickCommand(string buttonName, InputCommandType commandType) {
			RegisterButtonClickCommand(buttonName, new SimpleCommand(commandType));
		}
	}
}