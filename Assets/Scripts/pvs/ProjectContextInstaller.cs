using pvs.logic.playground;
using pvs.logic.playground.camera;
using pvs.logic.playground.state;
using pvs.logic.playground.state.building.settings;
using pvs.settings.debug;
using UnityEngine;
using Zenject;
namespace pvs {
	public class ProjectContextInstaller : MonoInstaller {
		// ReSharper disable Unity.PerformanceAnalysis
		public override void InstallBindings() {
			Debug.Log($"{GetType().Name}.InstallBindings()");

			BindDebugSettings();

			BindPlaygroundDependencies();
			BindPlaygroundCameraDependencies();
		}

		private void BindPlaygroundCameraDependencies() {
			Container.BindInterfacesAndSelfTo<PlaygroundCameraState>()
			         .AsSingle();
		}

		private void BindPlaygroundDependencies() {
			Container.BindInterfacesAndSelfTo<PlaygroundInitialState>()
			         .AsSingle();

			Container.BindInterfacesAndSelfTo<PlaygroundState>()
			         .AsSingle();
			
			Container.BindInterfacesAndSelfTo<BuildingsSettings>()
			         .AsSingle();
		}

		private void BindDebugSettings() {
			Container.Bind<DebugSettings>()
			         .FromComponentOn(GameObject.Find("DebugSettingsView"))
			         .AsSingle()
			         .NonLazy();
		}
	}
}