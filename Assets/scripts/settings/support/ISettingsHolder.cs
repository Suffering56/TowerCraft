namespace settings.support
{
	public interface ISettingsHolder
	{
		PlaygroundSettings PlaygroundSettings { get; }
		CameraSettings CameraSettings { get; }
	}
}