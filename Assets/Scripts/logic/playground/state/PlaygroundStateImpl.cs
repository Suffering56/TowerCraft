using System;
using System.Collections.Generic;
using logic.playground.State.Building;
using UnityEngine;

namespace logic.playground.state
{
	public class PlaygroundStateImpl : ScriptableObject
	{
		[SerializeField]
		private GameObject sword;
		
		private readonly IDictionary<Vector2, BuildingState> busyGridPoints = new Dictionary<Vector2, BuildingState>();
		private int buildingIdGenerator = 0;

		public int CreateBuilding(Vector2 gridPoint)
		{
			// IUnityContainer container = new UnityContainer();
			int buildingId = ++buildingIdGenerator;
			var building = new BuildingState();
			
			busyGridPoints.Add(gridPoint, building);
			
			return buildingId;
		}

		public bool IsBusyGridPoint(Vector2 gridPoint)
		{
			return busyGridPoints.ContainsKey(gridPoint);
		}

		private void Awake()
		{
			throw new NotImplementedException();
		}
	}
}