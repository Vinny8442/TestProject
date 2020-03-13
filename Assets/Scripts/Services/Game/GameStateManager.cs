using System;
using test.project;
using test.project.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
	public class GameStateManager : IGameStateManager
	{
		private IConfigStorage _configStorage;
		private GameConfig _gameConfig;
		private int _currentLevelIndex = -1;
		private IGameLoader _loader;

		public GameStateManager(IGameLoader loader)
		{
			_loader = loader;
		}

		public void Inject(IContainer container)
		{
			_configStorage = container.Get<IConfigStorage>();
		}

		public void Prepare()
		{
			
		}

		public void Start()
		{
			_gameConfig = _configStorage.Get<GameConfig>(0);
			NextLevel();
		}

		public LevelConfig CurrentLevelConfig { get; private set; }

		public void NextLevel()
		{
			_currentLevelIndex++;
			if (_currentLevelIndex < _gameConfig.Levels.Length)
			{
				var currentLevelId = _gameConfig.Levels[_currentLevelIndex];
				CurrentLevelConfig = _configStorage.Get<LevelConfig>(currentLevelId);
			}
			
			_loader.LoadScene<LevelInitialization>(CurrentLevelConfig.SceneName);
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}
	}
}