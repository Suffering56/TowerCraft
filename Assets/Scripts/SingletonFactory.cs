using System.Runtime.CompilerServices;
using Zenject;

public abstract class SingletonFactory<T> : PlaceholderFactory<T>
{
	private T instance;

	public override T Create()
	{
		return Get();
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public T Get()
	{
		if (instance == null)
		{
			instance = base.Create();
		}

		return instance;
	}
}