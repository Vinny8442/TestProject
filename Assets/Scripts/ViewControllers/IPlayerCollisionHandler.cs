using UnityEngine;

namespace test.project
{
	public interface IPlayerCollisionHandler
	{
		void OnCollision(GameObject other);
	}
}