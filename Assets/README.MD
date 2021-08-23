# Настройки (SettingsManager):

- для быстрого доступа к настройкам используй SettingsManager.Instance
- нельзя использовать настройки в методе Awake()
- если хочется использовать настройки в OnDrawGizmos() (который вызывается в редакторе, минуя Awake()), то нужно подписаться на ISettingsDebugRefreshListener и сохранить ссылку на ISettingsHolder(или
  его часть). Но в OnDrawGizmos() всегда нужно проверять эту ссылку на null.

- все папки с ассетами называем с большой буквы, кроме подпапок Scripts (будут жуткие конфликты имен)






- именования:
- controller/component - скрипт, прицепленный к gameObjectу и отвечающий за его управление
- state - runtime состояние чего-либо
- entity - персистентная сущность
- settings -> immutable настройки, прочитанные из json
- debugSettings -> настройки, позволяющие удобно менять сцену в onDrawGizmos (класс, который использует эти настройки, обязан подписаться на IDebugSettingsRefreshListener, а его родительский gameObject должен быть добавлен в DebugSettingsManager.ObservableGameObjects)
- settingsState-> пользовательские настройки (которые игрок выставил в меню настроек)



- пивот здания = bottom center
- пивот gridElement = top left