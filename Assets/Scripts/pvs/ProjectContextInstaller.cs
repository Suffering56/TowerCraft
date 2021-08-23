using System;
using System.Linq;
using ModestTree;
using pvs.logic.playground;
using pvs.logic.playground.building;
using pvs.logic.playground.building.settings;
using pvs.logic.playground.camera;
using pvs.settings.debug;
using pvs.utils;
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

			// float yDiff = 1.82f - 0.125f;
			// float xDiff = 1.2f - 0.25f;
			
			float x = -3.8f; // + 0.25f;
			float y = 3.18f; //+ 0.125f;

			var mousePos = new Vector2(x, y);

			var mayBeNearest = new Vector2(
				VMath.RoundTo(mousePos.x, 0.5f),
				VMath.RoundTo(mousePos.y, 0.25f)
			);

			var direction = mayBeNearest - mousePos;
			direction = new Vector2(Math.Sign(direction.x), Math.Sign(direction.y));

			var candidate2 = new Vector2(
				direction.x * 0.25f,
				direction.y * 0.125f
			);
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
				.AsSingle()
				.NonLazy();
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