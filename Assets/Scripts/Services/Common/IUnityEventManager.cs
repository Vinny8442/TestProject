using System.Collections;
using UnityEngine;

namespace test.project.Services
{
	public interface IUnityEventManager : IService
	{
		void Add(IUpdatable client);
		void Remove(IUpdatable client);
		void Add(IFixedUpdatable client);
		void Remove(IFixedUpdatable client);
		void Add(ILateUpdatable client);
		void Remove(ILateUpdatable client);
		Coroutine StartCoroutine(IEnumerator coroutine);
		void StopCoroutine(Coroutine coroutineHandler);
	}
	
	public interface IUpdatable
	{
		void OnUpdate(float deltaTime);
	}

	public interface IFixedUpdatable
	{
		void OnFixedUpdate(float deltaTime);
	}

	public interface ILateUpdatable
	{
		void OnLateUpdate(float deltaTime);
	}
}