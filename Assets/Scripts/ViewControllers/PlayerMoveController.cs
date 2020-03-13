using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test.project
{
	public class PlayerMoveController : MonoBehaviour
	{
		[SerializeField] private AnimationCurve _curve;
		
		private bool _isMoving = false;
		private float _timePassed = 0;
		private float _timeOverall = 0;
		private Vector3 _startPoint;
		private Vector3 _destPoint;
		private IPlayerCollisionHandler _collisionHandler;
		private Action _moveCompleteCallback;
		private Rigidbody _rigidbody;
		private float _colliderRadius = 1;
		private MoveMethod _moveMethod;

		private void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_colliderRadius = GetComponent<SphereCollider>().radius;
		}

		public void Move(MoveMethod moveMethod, Vector3 toPoint, float time, Action onMoveComplete)
		{
			_moveMethod = time > 0 ? moveMethod : MoveMethod.MovePosition; // FIXME a dirty hack 
			_moveCompleteCallback = onMoveComplete;
			_isMoving = true;
			_timePassed = 0;
			_timeOverall = time;
			_destPoint = toPoint;
			_startPoint = transform.position;
			_rigidbody.drag = 0;
			if (_moveMethod == MoveMethod.SetVelocityThenStop)
			{
				_rigidbody.velocity = _timeOverall > 0 ? (_destPoint - _startPoint) / _timeOverall : (_destPoint - _startPoint).normalized * short.MaxValue;
			}
			else if (_moveMethod == MoveMethod.ForcesWithDrag)
			{
				_rigidbody.AddForce((_destPoint - _startPoint) / _timeOverall, ForceMode.Acceleration);
				_rigidbody.drag = 1;
			}
		}
		
		private void FixedUpdate()
		{
			if (_moveMethod == MoveMethod.MovePosition)
			{
				if (_isMoving)
				{
					_timePassed += Time.fixedDeltaTime;
					float progress = GetProgress();

					Vector3 nextPoint = Vector3.Lerp(_startPoint, _destPoint, _curve.Evaluate(progress));
					Vector3 direction = nextPoint - transform.position;
					Ray ray = new Ray(transform.position, direction);
					if (Physics.SphereCast(ray, _colliderRadius, out var hit, direction.magnitude * 1f))
					{
						nextPoint = ray.GetPoint(hit.distance * 1.01f);
					}

					_rigidbody.MovePosition(nextPoint);

					if (progress == 1)
					{
						_isMoving = false;
						_moveCompleteCallback?.Invoke();
					}
				}
			}
			
			else if (_moveMethod == MoveMethod.ForcesWithDrag)
			{
				if (_isMoving && _rigidbody.velocity.magnitude == 0)
				{
					_isMoving = false;
					_moveCompleteCallback?.Invoke();
				}
			}

			else if (_moveMethod == MoveMethod.SetVelocityThenStop || _moveMethod == MoveMethod.ForcesWithDrag)
			{
				if (_isMoving && _timePassed > _timeOverall)
				{
					_isMoving = false;
					_rigidbody.velocity = Vector3.zero;
					_moveCompleteCallback?.Invoke();
				}

				// Vector3 direction = _rigidbody.velocity * Time.fixedDeltaTime;
				// Ray ray = new Ray(transform.position, direction);
				// if (Physics.SphereCast(ray, _colliderRadius, out var hit, 
				// 	Math.Min(direction.magnitude * 1f, (_destPoint - _startPoint).magnitude)
				// 	))
				// {
				// 	_rigidbody.velocity = Vector3.zero;
				// 	_rigidbody.MovePosition(ray.GetPoint(hit.distance * 1.01f));
				// 	_isMoving = false;
				// 	_moveCompleteCallback?.Invoke();
				// }

				_timePassed += Time.fixedDeltaTime;
			}
		}

		private void LateUpdate()
		{
		}

		public void SetCollisionHandler(IPlayerCollisionHandler collisionHandler)
		{
			_collisionHandler = collisionHandler;
		}

		private void OnTriggerEnter(Collider other)
		{
			_collisionHandler?.OnCollision(other.gameObject);
		}

		private void OnCollisionEnter(Collision other)
		{
			_collisionHandler?.OnCollision(other.gameObject);
		}

		private float GetProgress() => _timeOverall > 0 ? Math.Min(_timePassed / _timeOverall, 1f) : 1;

		public void Stop()
		{
			_isMoving = false;
			_rigidbody.velocity = Vector3.zero;
		}
	}
}