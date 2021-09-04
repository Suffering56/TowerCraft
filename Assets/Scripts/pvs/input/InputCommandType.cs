namespace pvs.input {

	public enum InputCommandType {
		SELECT_BUILDING_TEMPLATE,	// клик по зданию, которое будем строить на UI панели с доступными постройками
		DISABLE_BUILDING_MODE,	
		OPEN_BUILDINGS_LIST_PANEL,
		
		SELECT_BUILDING,			// обычный клик по зданию в режиме игры
		TERRAIN_CLICK,			    // обычный клик пустой территории в режиме игры
		SELL_BUILDING,
		
		RESET_BUILDINGS,
		SAVE_BUILDINGS
	}
}