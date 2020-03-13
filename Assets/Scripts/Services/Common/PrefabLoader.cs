using System.Collections.Generic;
using UnityEngine;

namespace test.project.Services
{
	public class PrefabLoader : IPrefabLoader
	{
		private Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();
		
		public void Inject(IContainer container)
		{
			
		}

		public void Prepare()
		{
		}

		public void Start()
		{
		}

		public void Reset()
		{
		}

		public void Clear()
		{
		}

		public T Get<T>(string linkage) where T : MonoBehaviour
		{
			GameObject result = Get(linkage);
			return result.GetComponent<T>();
		}

		public GameObject Get(string linkage)
		{
			if (!_prefabs.TryGetValue(linkage, out var prefab))
			{
				prefab = Resources.Load<GameObject>(linkage);
				_prefabs.Add(linkage, prefab);
			}

			GameObject result = GameObject.Instantiate(prefab);
			return result;
		}
	}
}