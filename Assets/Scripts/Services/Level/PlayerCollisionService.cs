using System;
using UnityEngine;

namespace  test.project.Services
{
	public class PlayerCollisionService : IPlayerCollisionService, IService, IPlayerCollisionHandler
	{
		private PlayerMoveController _player;
		private ILevelStateManager _levelStateManager;

		public PlayerCollisionService(PlayerMoveController player)
		{
			_player = player;
		}

		public void Inject(IContainer container)
		{
			_levelStateManager = container.Get<ILevelStateManager>();
		}

		public void Prepare()
		{
		}

		public void Start()
		{
			_player.SetCollisionHandler(this);
		}

		public void Reset()
		{
		}

		public void Clear()
		{
			_player.SetCollisionHandler(null);
		}

		public void OnCollision(GameObject other)
		{
			if (other.TryGetComponent<ObstacleMarker>(out var obstacle))
			{
				_levelStateManager.HandleObstacleCollision();
			}
			else if (other.TryGetComponent<LevelTargetMarker>(out var target))
			{
				_levelStateManager.HandleTargetCollision();
			}
		}
		 
	}
}