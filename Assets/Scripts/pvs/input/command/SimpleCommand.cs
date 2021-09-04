namespace pvs.input.command {

	public class SimpleCommand : IInputCommand {
		
		// TODO: нет смысла каждый раз создавать новый экземпляр. просто сделаем константы

		private readonly InputCommandType commandType;

		public SimpleCommand(InputCommandType commandType) {
			this.commandType = commandType;
		}

		public InputCommandType GetCommandType() {
			return commandType;
		}
	}
}