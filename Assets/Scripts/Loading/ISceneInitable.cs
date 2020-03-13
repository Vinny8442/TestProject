using test.project.Services;

namespace test.project
{
	public interface ISceneInitable
	{
		void Init(IContainer container);
		void Clear();
	}
}