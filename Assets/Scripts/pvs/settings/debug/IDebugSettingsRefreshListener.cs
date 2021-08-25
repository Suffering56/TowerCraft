using JetBrains.Annotations;
namespace pvs.settings.debug
{
	/**
	 * Интерфейс для слушателей обновления настроек проекта.
	 * Важно помнить, что при добавлении нового подписчика, нужно зарегистрировать родительский (для скрипта) GameObject в SettingsManager 
	 */
	public interface IDebugSettingsRefreshListener
	{
		/**
		 * Вызывается при изменении настроек в режиме редкатора по "MonoBehaviour.OnDrawGizmos()"
		 * НЕ МОЖЕТ БЫТЬ ВЫЗВАНО В РАНТАЙМЕ
		 */
		void OnDebugSettingsRefreshed([NotNull] DebugSettings debugSettings);
	}
}