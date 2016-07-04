using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TrackedRiderUtility
{
	public static class GameObjectHelper
	{
		public static GameObject SetUV(GameObject GO, int gridX, int gridY)
		{
			Mesh mesh = GO.GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			Vector2[] array = new Vector2[vertices.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Vector2(0.0625f * ((float)gridX + 0.5f), 1f - 0.0625f * ((float)gridY + 0.5f));
			}
			mesh.uv = array;
			return GO;
		}

		public static void RegisterDeprecatedMapping(string oldMapping, string newMapping)
		{
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField;
			FieldInfo field = typeof(AssetManager).GetField("deprecatedPrefabMapping", bindingAttr);
			Dictionary<string, string> dictionary = (Dictionary<string, string>)field.GetValue(ScriptableSingleton<AssetManager>.Instance);
            if(!dictionary.ContainsKey(oldMapping))
                dictionary.Add(oldMapping, newMapping);
		}
	}
}
