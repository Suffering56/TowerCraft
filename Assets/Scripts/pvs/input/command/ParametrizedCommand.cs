namespace pvs.input.command {

	public class ParametrizedCommand<T> : IInputCommand {

		private readonly InputCommandType commandType;
		private readonly T param;

		public ParametrizedCommand(InputCommandType type, T param) {
			this.commandType = type;
			this.param = param;
		}

		public InputCommandType GetCommandType() {
			return commandType;
		}

		public T GetParam() {
			return param;
		}
	}
}