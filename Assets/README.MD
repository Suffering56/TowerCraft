# Настройки (SettingsManager):

- для быстрого доступа к настройкам используй SettingsManager.Instance
- нельзя использовать настройки в методе Awake()
- если хочется использовать настройки в OnDrawGizmos() (который вызывается в редакторе, минуя Awake()), то нужно подписаться на ISettingsDebugRefreshListener и сохранить ссылку на ISettingsHolder(или
  его часть). Но в OnDrawGizmos() всегда нужно проверять эту ссылку на null.
