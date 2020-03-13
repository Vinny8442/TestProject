using System;
using System.Collections.Generic;
using UnityEngine;

namespace test.project
{
	[CreateAssetMenu(fileName = "new strings config", menuName = "__TestProject/StringsConfig", order = 0)]
	public class LocalesConfig : ScriptableObject
	{
		public Pair[] Strings;
		
		[Serializable]
		public class Pair
		{
			public string Key;
			public string Value;
		}
	}

	
}