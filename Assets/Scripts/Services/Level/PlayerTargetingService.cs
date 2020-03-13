using test.project.Services;
using UnityEngine;

namespace test.project.Services
{
	public class PlayerTargetingService : IPlayerTargetingService, IUpdatable
	{
		private IUnityEventManager _unityEventManager;
		private bool _enabled = true;
		private bool _started;
		private IPrefabLoader _prefabLoader;
		private ArrowController _arrow;
		private Camera _camera;
		private Plane _plane;
		private Vector3 _startPoint;
		private Vector3 _endPoint;
		private PlayerMoveController _player;
		private IPlayerMovementService _movementService;

		public PlayerTargetingService(PlayerMoveController player)
		{
			_player = player;
		}

		public void Inject(IContainer container)
		{
			_unityEventManager = container.Get<IUnityEventManager>();
			_prefabLoader = container.Get<IPrefabLoader>();
			_movementService = container.Get<IPlayerMovementService>();
		}

		public void Prepare()
		{
			_arrow = _prefabLoader.Get<ArrowController>(PrefabNames.Arrow);
			_arrow.gameObject.SetActive(false);
			_plane = new Plane(Vector3.back, 0);
		}

		public void Start()
		{
			_unityEventManager.Add(this);
			_camera = Camera.main;
		}

		public void Reset()
		{
		}	

		public void Clear()
		{
			_unityEventManager.Remove(this);
		}

		public void SetEnabled(bool value)
		{
			_enabled = value;
		}

		public void OnUpdate(float deltaTime)
		{

			if (!_enabled)
			{
				return;
			}
			
			if (!_started && Input.GetMouseButtonDown(0))
			{
				if (TryGetPoint(out _startPoint))
				{
					_started = true;
				}
			}
			
			else if (_started && Input.GetMouseButtonUp(0))
			{
				if (TryGetPoint(out _endPoint))
				{
					_started = false;
					var direction = _startPoint - _endPoint;
					_arrow.SetParams(Quaternion.FromToRotation(Vector3.left, direction), _player.transform.position, (_endPoint - _startPoint).magnitude);
					_arrow.gameObject.SetActive(false);
					CompleteAction(_startPoint, _endPoint);
				}
			}
			
			else if (_started)
			{
				if (TryGetPoint(out _endPoint))
				{
					var direction = _startPoint - _endPoint;
					if (direction.magnitude != 0)
					{
						_arrow.gameObject.SetActive(true);
						_arrow.SetParams(Quaternion.FromToRotation(Vector3.left, direction), _player.transform.position, (_endPoint - _startPoint).magnitude);
					}
				}
			}
		}

		private bool TryGetPoint(out Vector3 point)
		{
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (_plane.Raycast(ray, out var distance))
			{
				point = ray.GetPoint(distance);
				return true;
			}
			point = Vector3.zero;
			return false;
		}

		private void CompleteAction(Vector3 startPoint, Vector3 endPoint)
		{
			_movementService.Move(endPoint - startPoint);
		}
	}
}