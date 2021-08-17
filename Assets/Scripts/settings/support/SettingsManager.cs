using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace settings.support
{
	public class SettingsManager : MonoBehaviour, ISettingsHolder
	{
		public static SettingsManager Instance { get; private set; }

		[SerializeField]
		private bool forceRefresh = false;

		[SerializeField]
		private int refreshDelayMs = 1000;

		[SerializeField]
		private List<GameObject> objectWithListeners = new List<GameObject>();

		private readonly Stopwatch stopwatch = Stopwatch.StartNew();
		private long lastRefreshTime = 0;

		public PlaygroundSettings PlaygroundSettings { get; private set; }
		public CameraSettings CameraSettings { get; private set; }

		private void Awake()
		{
			if (Instance != null)
			{
				Debug.LogError("SettingsManager already initialized");
			}

			Instance = this;
			
			ReadSettings();
			Debug.Log("Awake: Settings initialized");
			
			OnSettingsRefreshed(false);
			Debug.Log("Awake: Settings refreshed");
		}

		private void OnDrawGizmos()
		{
			var currentTime = stopwatch.ElapsedMilliseconds;
			var delta = currentTime - lastRefreshTime;

			if (forceRefresh || delta > refreshDelayMs)
			{
				ReadSettings();
				OnSettingsRefreshed(forceRefresh);
				lastRefreshTime = currentTime;
				forceRefresh = false;
			}
		}

		private void OnSettingsRefreshed(bool force)
		{
			if (!objectWithListeners.Any()) return;

			var listeners = objectWithListeners
			.SelectMany(go => go.GetComponents<ISettingsDebugRefreshListener>())
			.ToList();

			foreach (var listener in listeners)
			{
				listener.OnSettingsRefreshed(this, force);
			}
		}

		private void ReadSettings()
		{
			PlaygroundSettings = GetComponent<PlaygroundSettings>();
			CameraSettings = GetComponent<CameraSettings>();
		}
	}
}