using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace pvs.settings.debug
{
	public class DebugSettingsManager : MonoBehaviour
	{
		private static DebugSettingsManager _instance;

		[SerializeField]
		private bool forceRefresh = false;

		[SerializeField]
		private int refreshDelayMs = 1000;

		[SerializeField]
		private List<GameObject> refreshSubscribers = new List<GameObject>();

		private readonly Stopwatch stopwatch = Stopwatch.StartNew();
		private long lastRefreshTime = 0;

		private void Awake()
		{
			Debug.Log($"{GetType()}.Awake()");
			
			if (_instance != null)
			{
				Debug.LogError("SettingsManager already initialized");
			}
			_instance = this;
		}

		private void OnDrawGizmos()
		{
			var currentTime = stopwatch.ElapsedMilliseconds;
			var delta = currentTime - lastRefreshTime;

			if (forceRefresh || delta > refreshDelayMs)
			{
				OnSettingsRefreshed(forceRefresh);
				lastRefreshTime = currentTime;
				forceRefresh = false;
			}
		}

		private void OnSettingsRefreshed(bool force)
		{
			if (!refreshSubscribers.Any()) return;

			var listeners = refreshSubscribers
			.SelectMany(go => go.GetComponents<IDebugSettingsRefreshListener>())
			.ToList();

			var debugSettings = GetComponent<DebugSettings>();

			foreach (var listener in listeners)
			{
				listener.OnDebugSettingsRefreshed(debugSettings, force);
			}
		}
	}
}