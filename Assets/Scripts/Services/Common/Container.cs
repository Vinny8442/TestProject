using System;
using System.Collections.Generic;

namespace test.project.Services
{
	public class Container : IContainer
	{
		// private static Container _instance;
		// public static Container Instance => _instance ?? (_instance = new Container());

		private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();
		private readonly IContainer _parent;

		public Container(IContainer parent)
		{
			_parent = parent;
		}

		public Container()
		{
		}
		
		public void Register<T>(IService service) where T : class
		{
			Type type = typeof(T);
			if (!_services.ContainsKey(type))
			{
				_services.Add(type, service);
			}
		}

		public T Get<T>() where T : class
		{
			Type type = typeof(T);
			if (_services.ContainsKey(type))
			{
				return _services[type] as T;
			}
			return _parent?.Get<T>();
		}

		public void ResetAll()
		{
			foreach (IService service in _services.Values)
			{
				service.Reset();
			}
		}

		public void Inject()
		{
			foreach (IService service in _services.Values)
			{
				service.Inject(this);
			}
		}
		
		public void PrepareAll()
		{
			foreach (IService service in _services.Values)
			{
				service.Prepare();
			}
		}

		public void StartAll()
		{
			foreach (IService service in _services.Values)
			{
				service.Start();
			}
		}

		public void Clear()
		{
			foreach (var service in _services.Values)
			{
				service.Clear();
			}
			_services.Clear();
		}
	}
}