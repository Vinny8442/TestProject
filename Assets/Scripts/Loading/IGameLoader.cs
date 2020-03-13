namespace test.project
{
	public interface IGameLoader
	{
		void LoadScene<T>(string sceneLinkage) where T:class, ISceneInitable;
	}
}