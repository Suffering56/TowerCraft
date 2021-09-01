namespace pvs.input.command {

	public class SimpleCommand : IInputCommand {

		private readonly InputCommandType commandType;
		private readonly bool fromUI;

		public SimpleCommand(InputCommandType commandType) {
			this.commandType = commandType;
		}

		public SimpleCommand(InputCommandType commandType, bool fromUI) {
			this.commandType = commandType;
			this.fromUI = fromUI;
		}

		public InputCommandType GetCommandType() {
			return commandType;
		}

		public bool IsFromUI() {
			return fromUI;
		}

		public override string ToString() {
			return $"{nameof(commandType)}: {commandType}, {nameof(fromUI)}: {fromUI}";
		}
	}
}