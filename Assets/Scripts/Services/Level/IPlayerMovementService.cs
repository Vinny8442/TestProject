using UnityEngine;

namespace test.project.Services
{
	public interface IPlayerMovementService : IService
	{
		void Move(Vector3 startPoint);
		void Stop();
	}
}