namespace pvs.input.command {

	public class SimpleCommand : IInputCommand {

		private readonly InputCommandType commandType;

		public SimpleCommand(InputCommandType commandType) {
			this.commandType = commandType;
		}

		public InputCommandType GetCommandType() {
			return commandType;
		}
	}
}