using test.project.Services;
using UnityEngine;

namespace test.project
{
	public class LevelInitialization : MonoBehaviour, ISceneInitable
	{
		[SerializeField] private PlayerMoveController playerMove;
		[SerializeField] private InterfaceController _interface;
		
		private Container _container;

		public void Init(IContainer parent)
		{
			_container = new Container(parent);

			_container.Register<IPlayerTargetingService>(new PlayerTargetingService(playerMove));
			_container.Register<IPlayerMovementService>(new PlayerMovementService(playerMove));
			_container.Register<IPlayerCollisionService>(new PlayerCollisionService(playerMove));
			_container.Register<ILevelStateManager>(new LevelStateManager(_interface, GameObject.FindObjectsOfType<ColorBlinker>()));

			_container.Inject();
			_container.PrepareAll();
			_container.StartAll();
		}

		public void Clear()
		{
			_container.Clear();
		}
	}
}