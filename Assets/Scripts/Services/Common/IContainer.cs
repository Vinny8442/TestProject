namespace test.project.Services
{
	public interface IContainer
	{
		T Get<T>() where T : class;
		
	}
}