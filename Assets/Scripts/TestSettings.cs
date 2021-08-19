using logic.playground.state;
using Zenject;
namespace logic.playground.camera.settings {
	
	public class TestSettings {

		private int x;
		[Inject] private readonly PlaygroundState playgroundState;

		public TestSettings(int x) {
			this.x = x;
		}
	}
}