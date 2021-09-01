namespace pvs.input {
	
	public interface IInputCommand {

		public InputCommandType GetCommandType();

		bool IsFromUI();
	}
}