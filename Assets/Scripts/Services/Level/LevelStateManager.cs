using System;
using System.Collections.Generic;
using DefaultNamespace;
using test.project;
using test.project.Services;
using UnityEngine;

namespace test.project.Services
{
	public class LevelStateManager : ILevelStateManager, IService, InterfaceController.IInterfaceControllerHandler
	{
		private InterfaceController _interfaceController;
		private StateEnum _currentState;
		private IPlayerTargetingService _playerTargetingService;
		private IPlayerMovementService _playerMovementService;
		private IPlayerCollisionService _playerCollisionService;
		private IGameStateManager _gameStateManager;
		private IConfigStorage _configStorage;
		private Color _failBlinkColor;

		private ColorBlinker[] _failBlinkers;


		enum StateEnum
		{
			GameInProgress,
			Win,
			Lose
		}
		
		public LevelStateManager(InterfaceController interfaceController, ColorBlinker[] failBlinkers)
		{
			_failBlinkers = failBlinkers;
			_interfaceController = interfaceController;
		}

		public void Inject(IContainer container)
		{
			_playerTargetingService = container.Get<IPlayerTargetingService>();
			_playerMovementService = container.Get<IPlayerMovementService>();
			_playerCollisionService = container.Get<IPlayerCollisionService>();
			_gameStateManager = container.Get<IGameStateManager>();
			_configStorage = container.Get<IConfigStorage>();
		}

		public void Prepare()
		{
			_failBlinkColor = _configStorage.Get<GameConfig>().FailBlinkColor;
		}

		public void Start()
		{
			_interfaceController.Hide();
			_interfaceController.SetHandler(this);
			_currentState = StateEnum.GameInProgress;
			_playerTargetingService.SetEnabled(true);
		}

		public void Reset()
		{
			Start();
		}

		public void Clear()
		{
			// throw new System.NotImplementedException();
		}
		
		public void HandleObstacleCollision()
		{
			if (_currentState == StateEnum.GameInProgress)
			{
				_interfaceController.SetText(_configStorage.GetString(LocalesEnum.Failed));
				_interfaceController.SetButtonText(_configStorage.GetString(LocalesEnum.TryAgain));
				_interfaceController.Show();
				_currentState = StateEnum.Lose;
				
				_playerTargetingService.SetEnabled(false);
				_playerMovementService.Stop();

				_failBlinkers?.ForEach(blinker => blinker.Blink(_failBlinkColor));
			}
		}

		public void HandleTargetCollision()
		{
			if (_currentState == StateEnum.GameInProgress)
			{
				_interfaceController.SetText(_configStorage.GetString(LocalesEnum.Won));
				_interfaceController.SetButtonText(_configStorage.GetString(LocalesEnum.NextLevel));
				_interfaceController.Show();
				_currentState = StateEnum.Win;
				
				_playerTargetingService.SetEnabled(false);
				_playerMovementService.Stop();
			}
		}

		public void HandleButtonClick()
		{
			if (_currentState == StateEnum.Win)
			{
				GotoNextLevel();
			}
			else if (_currentState == StateEnum.Lose)
			{
				TryAgain();
			}
		}

		private void TryAgain()
		{
			_failBlinkers?.ForEach(blinker => blinker.Restore());

			_playerCollisionService.Reset();
			_playerMovementService.Reset();
			_playerTargetingService.Reset();
			Reset();
		}

		private void GotoNextLevel()
		{
			_gameStateManager.NextLevel();
		}
	}

	internal static class BlinkerExtension
	{
		public static void ForEach(this IEnumerable<ColorBlinker> blinkers, Action<ColorBlinker> action)
		{
			foreach (ColorBlinker blinker in blinkers)
			{
				action.Invoke(blinker);
			}
		}
	}
}