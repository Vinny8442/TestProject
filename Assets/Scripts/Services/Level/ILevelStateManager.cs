using test.project;
using UnityEngine;

namespace test.project.Services
{
	public interface ILevelStateManager : IService
	{
		void HandleObstacleCollision();
		void HandleTargetCollision();
	}
}