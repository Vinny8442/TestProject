using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ResourcesPostprocessor : AssetPostprocessor
{
	private const string PrefabsPath = "Assets/Resources/Prefabs";
	private const string ResourcesPath = "Assets/Resources/";
	private const string PrefabFilter = "t:prefab";
	private const string PrefabExtension = ".prefab";
	private const string GeneratedScriptPath = "Scripts/Generated/PrefabNames.cs";
	private const string Template = "%name%\n{\n\t%contents%\n}";
	private static readonly string[] DisallowedNameSymbols = new[] {" ", ")", "("};

	public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
		string[] movedFromAssetPaths)
	{
		if (HasExtension(importedAssets, PrefabExtension) || HasExtension(deletedAssets,PrefabExtension) || HasExtension(movedAssets,PrefabExtension))
		{
			GenerateAssetsList(PrefabFilter, GeneratedScriptPath);
		}
	}

	private static bool HasExtension(string[] list, string ext)
	{
		foreach (string localPath in list)
		{
			if (localPath.EndsWith(ext))
			{
				return true;
			}
		}

		return false;
	}

	private static void GenerateAssetsList(string filter, string filepath)
	{
		string[] prefabs = AssetDatabase.FindAssets(filter, new[] {PrefabsPath});
		string[] prefabPaths = prefabs.Convert(
			AssetDatabase.GUIDToAssetPath).Convert(path =>
			{
				string shorter = string.Join("/", path.Split("/".ToCharArray()).Skip(2));
				return shorter.Substring(0, shorter.LastIndexOf("."));
			});

		StringBuilder sb = new StringBuilder();
		foreach (string prefabPath in prefabPaths)
		{
			string prefabName = prefabPath.Substring(prefabPath.LastIndexOf("/") + 1);
			sb.Append($"public static readonly string {ReplaceDisallowedSymbols(prefabName)} = \"{prefabPath}\";\n");
		}

		string className = filepath.Substring(filepath.LastIndexOf("/") + 1);
		className = className.Substring(0, className.IndexOf("."));
		filepath = $"{Application.dataPath}/{filepath}";
		File.WriteAllText(filepath, "// Generated code\n" + 
		                            ApplyTemplate(
			                            "namespace test.project", 
			                            ApplyTemplate($"public static class {className}", sb.ToString())
		                            )
		);
	}

	private static string ApplyTemplate(string name, string contents)
	{
		return Template
			.Replace("%name%", name)
			.Replace("%contents%", contents.Replace("\n", "\n\t"));
	}

	private static string ReplaceDisallowedSymbols(string value)
	{
		foreach (string symbol in DisallowedNameSymbols)
		{
			value = value.Replace(symbol, "");
		}

		return value;
	}
}

internal static class ArrayExt
{
	public static TResult[] Convert<TSource, TResult>(this TSource[] source, Func<TSource, TResult> func)
	{
		TResult[] result = new TResult[source.Length];
		for (var i = 0; i < source.Length; i++)
		{
			result[i] = func(source[i]);
		}

		return result;
	}
}