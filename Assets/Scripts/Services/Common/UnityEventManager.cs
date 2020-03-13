using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test.project.Services
{
	public class UnityEventManager : IUnityEventManager
	{
		public bool IsReady { get; private set; }

		private readonly List<IUpdatable> _updates = new List<IUpdatable>();
		private readonly List<IFixedUpdatable> _fixedUpdates = new List<IFixedUpdatable>();
		private readonly List<ILateUpdatable> _lateUpdates = new List<ILateUpdatable>();
		private UpdateManagerBehaviour _updater;

		public void Add(IUpdatable client)
		{
			if (!_updates.Contains(client))
			{
				_updates.Add(client);
			}
		}

		public void Remove(IUpdatable client)
		{
			if (_updates.Contains(client))
			{
				_updates.Remove(client);
			}
		}

		public void Add(IFixedUpdatable client)
		{
			if (!_fixedUpdates.Contains(client))
			{
				_fixedUpdates.Add(client);
			}
		}

		public void Remove(IFixedUpdatable client)
		{
			if (_fixedUpdates.Contains(client))
			{
				_fixedUpdates.Remove(client);
			}
		}

		public void Add(ILateUpdatable client)
		{
			if (!_lateUpdates.Contains(client))
			{
				_lateUpdates.Add(client);
			}
		}

		public void Remove(ILateUpdatable client)
		{
			if (_lateUpdates.Contains(client))
			{
				_lateUpdates.Remove(client);
			}
		}

		public Coroutine StartCoroutine(IEnumerator coroutine)
		{
			return _updater.StartCoroutine(coroutine);
		}

		public void StopCoroutine(Coroutine coroutineHandler)
		{
			_updater.StopCoroutine(coroutineHandler);
		}


		public void Inject(IContainer container)
		{
		}

		public void Prepare()
		{
			var updaterGO = new GameObject("UpdateManager");
			GameObject.DontDestroyOnLoad(updaterGO);
			_updater = updaterGO.AddComponent<UpdateManagerBehaviour>();
			_updater.Init(OnUpdate, OnFixedUpdate, OnLateUpdate);
			IsReady = true;
		}

		public void Start()
		{
		}

		public void Reset()
		{ 
		}

		public void Clear()
		{
			_updates.Clear();
			_fixedUpdates.Clear();
			_lateUpdates.Clear();
			GameObject.Destroy(_updater.gameObject);
			_updater = null;
		}

		private void OnUpdate(float deltaTime)
		{
			var clients = _updates.ToArray();
			foreach (IUpdatable client in clients)
			{
				client.OnUpdate(deltaTime);
			}
		}

		private void OnFixedUpdate(float deltaTime)
		{
			var clients = _fixedUpdates.ToArray();
			foreach (IFixedUpdatable client in clients)
			{
				client.OnFixedUpdate(deltaTime);
			}
		}

		private void OnLateUpdate(float deltaTime)
		{
			var clients = _lateUpdates.ToArray();
			foreach (ILateUpdatable client in clients)
			{
				client.OnLateUpdate(deltaTime);
			}
		}
	}

	internal class UpdateManagerBehaviour : MonoBehaviour
	{
		private Action<float> _updateCallback;
		private Action<float> _fixedUpdateCallback;
		private Action<float> _lateUpdateCallback;

		public void Init(Action<float> updateCallback, Action<float> fixedUpdateCallback, Action<float> lateUpdateCallback)
		{
			_lateUpdateCallback = lateUpdateCallback;
			_updateCallback = updateCallback;
			_fixedUpdateCallback = fixedUpdateCallback;
		}

		private void Update()
		{
			_updateCallback?.Invoke(Time.deltaTime);
		}

		private void FixedUpdate()
		{
			_fixedUpdateCallback?.Invoke(Time.fixedDeltaTime);
		}

		private void LateUpdate()
		{
			_lateUpdateCallback?.Invoke(Time.deltaTime);
		}
	}
}