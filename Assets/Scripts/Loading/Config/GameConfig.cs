using UnityEngine;
using UnityEngine.SceneManagement;

namespace test.project
{
	[CreateAssetMenu(fileName = "New Game Config", menuName = "__TestProject/GameConfig", order = 0)]
	public class GameConfig : ScriptableObject, IConfigData
	{
		public int[] Levels;
		public string PreloaderScene;
		public MoveMethod MoveMethod;
		public Color FailBlinkColor;
		
		public int GetId() => 0;
	}

	public enum MoveMethod
	{
		MovePosition,
		ForcesWithDrag,
		SetVelocityThenStop
	}
}