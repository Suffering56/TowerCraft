using System;
namespace pvs.utils.code {

	/*
	 * Маркирует класс о том, что это не просто pojo, а полноценный компонент зарегистрированный в Zenject
	 * и в который можно [Inject]-ить другие компоненты.
	 *
	 * Никакой функциональности этот маркер сам по себе не несет.
	 */
	public class ZenjectComponent : Attribute { }
}