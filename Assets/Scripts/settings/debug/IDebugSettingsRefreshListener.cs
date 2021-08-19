using JetBrains.Annotations;

namespace settings.debug
{
	/**
	 * Интерфейс для слушателей обновления настроек проекта.
	 * Важно помнить, что при добавлении нового подписчика, нужно зарегистрировать родительский (для скрипта) GameObject в SettingsManager 
	 */
	public interface IDebugSettingsRefreshListener
	{
		/**
		 * Вызывается о событию "MonoBehaviour.OnDrawGizmos()", но только при рефреше настроек (переключении флагша refresh в false в редакторе Unity)
		 */
		void OnDebugSettingsRefreshed([NotNull] DebugSettings debugSettings, bool isForce);
	}
}