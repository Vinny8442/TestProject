using test.project;

namespace DefaultNamespace
{
	public interface IGameStateManager : IService
	{
		void NextLevel();
		LevelConfig CurrentLevelConfig { get; }
	}
}