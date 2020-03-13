using System.Security.Principal;
using UnityEngine;

namespace test.project
{
	[CreateAssetMenu(fileName = "new Level Config", menuName = "__TestProject/LevelConfig", order = 0)]
	public class LevelConfig : ScriptableObject, IConfigData
	{
		public string SceneName;
		public float MoveTime;
		public int Id;
		public int GetId() => Id;
	}
}