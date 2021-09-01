namespace pvs.input.command {
	
	public class ParametrizedCommand<T>: IInputCommand {

		private readonly T param;
		private readonly bool fromUI;

		public ParametrizedCommand(T param, bool fromUI) {
			this.param = param;
			this.fromUI = fromUI;
		}

		public InputCommandType GetCommandType() {
			return InputCommandType.SELECT_BUILDING_TEMPLATE;
		}

		public bool IsFromUI() {
			return fromUI;
		}

		public T GetParam() {
			return param;
		}
	}
}