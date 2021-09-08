using System;
using pvs.logic.playground.isometric;
using pvs.settings.debug;
using pvs.utils;
using UnityEngine;
using Zenject;

namespace pvs.logic.playground.npc {

	public class NpcUnitController : MonoBehaviour {

		[SerializeField] private float speed = 1;
		
		private Animator animator;
		private SpriteRenderer spriteRenderer;
		private Vector3 moveDirection;

		[Inject] private IIsometricInfo isometricInfo;
		[Inject] private IPlaygroundInitialState playgroundInitialState;

		private static readonly int STAY = Animator.StringToHash("stay");
		private static readonly int GO_RIGHT = Animator.StringToHash("goRight");
		private static readonly int GO_LEFT = Animator.StringToHash("goLeft");

		private bool staying = false;
		private void Start() {
			animator = GetComponentInChildren<Animator>();
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			moveDirection = new Vector3(1, -0.5f, transform.position.z);
			animator.SetTrigger(GO_RIGHT);
		}

		private void Update() {
			if (Input.GetKeyUp(KeyCode.Space)) {
				staying = !staying;
				UpdateCurrentAnimation();
			}

			if (staying) return;


			var pos = transform.position;
			var newPosition = pos + moveDirection * (speed * Time.deltaTime);

			if (isometricInfo.IsOutOfGrid(newPosition, out var outDirection)) {
				var nearestBorder = isometricInfo.GetNearestGridElementCenter(pos);

				if (nearestBorder.HasValue) {
					newPosition = nearestBorder.Value.ToVector3(pos.z);
					moveDirection = ChangeMoveDirection(outDirection);
					UpdateCurrentAnimation();
				} else {
					// ReSharper disable once Unity.PerformanceCriticalCodeInvocation
					Debug.LogError($"nearest border not found for point = {pos}");
				}
			}

			transform.position = newPosition;
			spriteRenderer.sortingOrder = isometricInfo.CalculateSortingOrder(newPosition.y);
		}

		private void UpdateCurrentAnimation() { animator.SetTrigger(GetAnimation()); }

		private int GetAnimation() {
			if (staying) {
				return STAY;
			}
			return moveDirection.x > 0 ? GO_RIGHT : GO_LEFT;
		}

		private Vector3 ChangeMoveDirection(Direction outDirection) {
			switch (outDirection) {
				case Direction.TOP:
				case Direction.BOTTOM:
					return new Vector3(moveDirection.x * RandomSign(), -moveDirection.y, moveDirection.z);

				case Direction.LEFT:
				case Direction.RIGHT: return new Vector3(-moveDirection.x, moveDirection.y * RandomSign(), moveDirection.z);

				default: throw new ArgumentOutOfRangeException(nameof(outDirection), outDirection, null);
			}
		}

		private static int RandomSign() {
			// var random = new Random().Next(0, 2);
			// return random == 0 ? -1 : 1;
			return 1;
		}
	}
}