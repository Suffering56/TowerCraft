using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace pvs.settings.debug {
	public class DebugSettingsManager : MonoBehaviour {

		[SerializeField]
		public bool triggerRefresh = false;

		[SerializeField]
		private List<GameObject> refreshSubscribers = new List<GameObject>();
		
		private static DebugSettingsManager _instance;

		private bool runtime = false;

		private void Awake() {
			Debug.Log($"{GetType().Name}.Awake()");

			if (_instance != null) {
				Debug.LogError($"{GetType().Name} already initialized");
			}

			_instance = this;
			runtime = true;

			GameObject.Find("Point000").SetActive(false);
		}

		private void OnDrawGizmos() {
			if (runtime) return;
			
			if (triggerRefresh) {
				OnSettingsRefreshed();
				triggerRefresh = false;
				Debug.Log("Debug settings refreshed");
			}
		}

		private void OnValidate() {
			triggerRefresh = true;
		}

		private void OnSettingsRefreshed() {
			if (!refreshSubscribers.Any()) return;

			var listeners = refreshSubscribers
			                .SelectMany(go => go.GetComponents<IDebugSettingsRefreshListener>())
			                .ToList();

			var debugSettings = GetComponent<DebugSettings>();

			foreach (var listener in listeners) {
				listener.OnDebugSettingsRefreshed(debugSettings);
			}
		}
	}
}