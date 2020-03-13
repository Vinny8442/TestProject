namespace test.project
{
	public interface IService : IInjectable
	{
		void Prepare();
		void Start();
		void Reset();
		void Clear();
		// bool IsReady { get; }
	}
}