using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using test.project.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace test.project
{
	public class GameLoader : MonoBehaviour, IGameLoader
	{
		private ConfigStorage _configStorage;
		private IUnityEventManager _unityEventManager;
		private Container _container;
		private LevelInitialization _levelInitialization;

		private void Start()
		{
			_container = new Container();

			_configStorage = new ConfigStorage();
			_unityEventManager = new UnityEventManager();
			
			_container.Register<IUnityEventManager>(_unityEventManager);
			_container.Register<IPrefabLoader>(new PrefabLoader());
			_container.Register<IConfigStorage>(_configStorage);

			GameStateManager gameStateManager = new GameStateManager(this);
			_container.Register<IGameStateManager>(gameStateManager);

			_container.Inject();
			_container.PrepareAll();
			_container.StartAll();
		}

		public void LoadScene<T>(string sceneLinkage) where T : class, ISceneInitable
		{
			var gameConfig = _configStorage.Get<GameConfig>(0);
			_unityEventManager.StartCoroutine(SceneLoaderCoroutine(gameConfig.PreloaderScene, sceneLinkage));
		}

		private IEnumerator SceneLoaderCoroutine(string preloaderLinkage, string sceneLinkage)
		{
			_levelInitialization?.Clear();
			SceneManager.LoadScene(preloaderLinkage);
			yield return SceneManager.LoadSceneAsync(sceneLinkage);
			_levelInitialization = FindObjectOfType<LevelInitialization>();
			if (_levelInitialization != null)
			{
				_levelInitialization.Init(_container);
			}
		}
	}
}