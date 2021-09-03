using JetBrains.Annotations;

namespace pvs.ui.utils {

	public interface ICursorVisibilityProvider {
		
		bool IsCursorVisible();

		void RegisterVisibilityChangeListener([NotNull] ICursorVisibilityChangeListener listener);
	}
}