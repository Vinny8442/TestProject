using DefaultNamespace;
using UnityEngine;

namespace test.project.Services
{
	public class PlayerMovementService : IPlayerMovementService, IService
	{
		private PlayerMoveController _playerMove;
		private Vector3 _initialPosition;
		private IGameStateManager _gameStateManager;
		private bool _moveInProgress = false;
		private IConfigStorage _configs;
		private MoveMethod _moveMethod;

		public PlayerMovementService(PlayerMoveController player)
		{
			_playerMove = player;
		}
		
		public void Inject(IContainer container)
		{
			_gameStateManager = container.Get<IGameStateManager>();
			_configs = container.Get<IConfigStorage>();
		}

		public void Prepare()
		{
			_initialPosition = _playerMove.transform.position;
			_moveMethod = _configs.Get<GameConfig>(0).MoveMethod;
		}

		public void Start()
		{
		}

		public void Reset()
		{
			_playerMove.transform.position = _initialPosition;
		}

		public void Clear()
		{
		}

		public void Move(Vector3 movement)
		{
			if (!_moveInProgress)
			{
				_moveInProgress = true;
				Debug.Log($"PlayerMovementService.Move");
				_playerMove.Move(_moveMethod, _playerMove.transform.position + movement, _gameStateManager.CurrentLevelConfig.MoveTime, OnMoveComplete);
			}
			else
			{
				Debug.Log($"PlayerMovementService: move already is in progress");
			}
		}

		public void Stop()
		{
			_moveInProgress = false;
			_playerMove.Stop();
		}

		private void OnMoveComplete()
		{
			Debug.Log($"PlayerMovementService: Stop callback");
			_moveInProgress = false;
		}
	}
}