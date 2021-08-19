using logic.playground.camera.settings;
using logic.playground.state;
using settings;
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

		// Container.BindFactory<PlaygroundState, PlaygroundState.Provider>()
		//          .AsSingle();

		Container.Bind<PlaygroundState>()
		         .AsSingle()
		         .NonLazy();
		Container.Bind<TestSettings>()
		         .AsSingle()
		         .WithArguments(22)
		         .NonLazy();
	}

	private void BindDebugSettings()
	{
		Container.Bind<DebugSettings>()
		         .FromComponentOn(GameObject.Find("DebugSettingsView"))
		         .AsSingle()
		         .NonLazy();
	}
}