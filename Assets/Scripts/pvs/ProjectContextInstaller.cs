using pvs.logic.playground.camera;
using pvs.logic.playground.state;
using pvs.settings.debug;
using UnityEngine;
using Zenject;
namespace pvs {
	public class ProjectContextInstaller : MonoInstaller
	{
		// ReSharper disable Unity.PerformanceAnalysis
		public override void InstallBindings()
		{
			Debug.Log($"{GetType().Name}.InstallBindings()");

			BindDebugSettings();
			BindPlaygroundState();
			BindCameraState();
		}

		private void BindCameraState() {
			Container.BindInterfacesAndSelfTo<PlaygroundCameraState>()
			         .AsSingle();
		}

		private void BindPlaygroundState() {
			Container.BindInterfacesAndSelfTo<PlaygroundInitialState>()
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
}