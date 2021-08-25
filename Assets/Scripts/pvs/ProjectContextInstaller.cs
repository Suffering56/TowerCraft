using System;
using System.Linq;
using ModestTree;
using pvs.logic.playground;
using pvs.logic.playground.building;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.camera;
using pvs.logic.playground.isometric;
using pvs.settings.debug;
using pvs.utils.code;
using UnityEngine;
using Zenject;

namespace pvs {

	public class ProjectContextInstaller : MonoInstaller {

		// ReSharper disable Unity.PerformanceAnalysis
		public override void InstallBindings() {
			Debug.Log($"{GetType().Name}.InstallBindings()");

			BindDebugSettings();
			BindPlaygroundCameraDependencies();
			BindPlaygroundDependencies();
		}

		private void BindDebugSettings() {
			Bind<DebugSettings>()
				.FromComponentOn(GameObject.Find("DebugSettingsView"))
				.AsSingle()
				.NonLazy();
		}
		
		private void BindPlaygroundCameraDependencies() {
			BindInterfacesAndSelfTo<PlaygroundCameraState>()
				.AsSingle();
		}

		private void BindPlaygroundDependencies() {
			BindInterfacesAndSelfTo<PlaygroundInitialState>()
				.AsSingle();

			BindInterfacesAndSelfTo<PlaygroundBuildingsState>()
				.AsSingle();

			BindInterfacesAndSelfTo<BuildingsSettings>()
				.AsSingle();
			
			BindInterfacesAndSelfTo<IsometricInfo>()
				.AsSingle();
		}

		private FromBinderNonGeneric BindInterfacesAndSelfTo<T>() {
			CheckVComponent<T>();
			return Container.BindInterfacesAndSelfTo<T>();
		}

		private ConcreteIdBinderGeneric<TContract> Bind<TContract>() {
			CheckVComponent<TContract>();
			return Container.Bind<TContract>();
		}

		/*
		 * Каждый регистрируемый компонент должен быть помечен [ZenjectComponent]
		 */
		private static void CheckVComponent<T>() {
			var type = typeof(T);
			var attributeExist = type.AllAttributes<ZenjectComponent>().Any();

			if (!attributeExist) {
				throw new Exception($"bindable attribute has no {nameof(ZenjectComponent)} annotation");
			}
		}
	}
}