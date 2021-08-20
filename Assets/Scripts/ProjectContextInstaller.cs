using logic.playground.state;
using settings.debug;
using UnityEngine;
using Zenject;

public class ProjectContextInstaller : MonoInstaller
{
	// ReSharper disable Unity.PerformanceAnalysis
	public override void InstallBindings()
	{
		Debug.Log($"{GetType()}.InstallBindings()");

		BindDebugSettings();
		BindPlaygroundState();
	}
	private void BindPlaygroundState() {
		Container.Bind<PlaygroundState>()
		         .AsSingle();
	}

	private void BindDebugSettings()
	{
		Container.Bind<DebugSettings>()
		         .FromComponentOn(GameObject.Find("DebugSettingsView"))
		         .AsSingle()
		         .NonLazy();
	}
}