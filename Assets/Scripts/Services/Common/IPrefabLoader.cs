using UnityEngine;

namespace test.project.Services
{
	public interface IPrefabLoader : IService
	{
		T Get<T>(string linkage) where T : MonoBehaviour;
		GameObject Get(string linkage);
	}
}