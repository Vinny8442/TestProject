using System;
using System.Collections.Generic;
using test.project;
using test.project.Services;
using UnityEngine;

namespace  test.project.Services
{
	public class ConfigStorage : IConfigStorage, IService
	{
		private readonly Dictionary<Type, Dictionary<int, project.IConfigData>> _storage = new Dictionary<Type, Dictionary<int, project.IConfigData>>();
		private readonly Dictionary<string, string> _strings = new Dictionary<string, string>();
		public void Inject(IContainer container)
		{
		}

		public void Prepare()
		{
			var levelConfigs = Resources.LoadAll<LevelConfig>("Config");
			ParseConfigs(levelConfigs);
			var gameConfigs = Resources.LoadAll<GameConfig>("Config");
			ParseConfigs(gameConfigs);
			
			var stringConfigs = Resources.LoadAll<LocalesConfig>("Config");
			ParseLocales(stringConfigs);
		}

		public void Start()
		{
		}

		public void Reset()
		{
		}

		public void Clear()
		{
			_storage.Clear();
		}

		public T Get<T>(int id) where T: class, project.IConfigData
		{
			if (_storage.TryGetValue(typeof(T), out var typedStorage))
			{
				if (typedStorage.TryGetValue(id, out var configData))
				{
					return configData as T;
				}
			}

			return null;
		}

		public T Get<T>() where T : class, IConfigData
		{
			if (_storage.TryGetValue(typeof(T), out var typedStorage))
			{
				foreach (IConfigData data in typedStorage.Values)
				{
					return data as T;
				}
			}
			return null;
		}

		public string GetString(string key)
		{
			if (_strings.TryGetValue(key, out var result))
			{
				return result;
			}

			return key;
		}

		private void ParseLocales(LocalesConfig[] stringConfigs)
		{
			foreach (LocalesConfig config in stringConfigs)
			{
				foreach (var stringPair in config.Strings)
				{
					_strings[stringPair.Key] = stringPair.Value;
				}
			}
		}

		private void ParseConfigs<T>(IEnumerable<T> configs) where T:class, project.IConfigData
		{
			Type cType = typeof(T);
			if (!_storage.TryGetValue(cType, out var typedStorage))
			{
				_storage[cType] = typedStorage = new Dictionary<int, project.IConfigData>();
			}

			foreach (T config in configs)
			{
				typedStorage[config.GetId()] = config;
			}
		}
	}
}