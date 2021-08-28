using System;
using System.IO;
using UnityEngine;

namespace pvs.utils {

	public static class JsonUtils {

		private static string GetPath(string fileName) {
			#if UNITY_EDITOR
			return Path.Combine(Application.dataPath, fileName);
			#else
			return Path.Combine(Application.persistentDataPath, fileName);
			#endif
		}

		public static void WriteJson<T>(string fileName, T jsonNode) {
			var json = JsonUtility.ToJson(jsonNode, true);
			var savePath = GetPath(fileName);

			try {
				File.WriteAllText(savePath, json);
			}
			catch (Exception e) {
				Debug.LogError($"fail to write json to {savePath}, message={e.Message}");
			}
		}

		public static T ReadJson<T>(string fileName) {
			var path = GetPath(fileName);

			if (!File.Exists(path)) {
				Debug.LogWarning($"couldn't read from file: {path}, because file not exist");
				return default;
			}

			try {
				string jsonString = File.ReadAllText(path);
				return JsonUtility.FromJson<T>(jsonString);
			}
			catch (Exception e) {
				Debug.LogWarning($"couldn't read from file: {path}, errorMsg ={e.Message}");
				return default;
			}
		}
	}
}