using test.project;

namespace  test.project.Services
{
	public interface IConfigStorage
	{
		T Get<T>(int id) where T : class, project.IConfigData;
		T Get<T>() where T : class, project.IConfigData;
		string GetString(string key);
	}
}