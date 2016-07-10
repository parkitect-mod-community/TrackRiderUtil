using System;
using UnityEngine;

namespace TrackedRiderUtility
{
	public static class TrackRideHelper
	{
		public static void MakeRecolorble(GameObject GO, string shader, Color[] colors)
		{
			CustomColors customColors = GO.AddComponent<CustomColors>();
			customColors.setColors(colors);
			Material[] objectMaterials = ScriptableSingleton<AssetManager>.Instance.objectMaterials;
			for (int i = 0; i < objectMaterials.Length; i++)
			{
				Material material = objectMaterials[i];
				if (material.name == shader)
				{
					TrackRideHelper.SetMaterial(GO, material);
					break;
				}
			}
		}

		public static void SetMaterial(GameObject go, Material material)
		{
			Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Renderer renderer = array[i];
				renderer.sharedMaterial = material;
			}
		}

		public static TrackedRide GetTrackedRide(string name)
		{
			TrackedRide original = null;
			foreach (Attraction current in ScriptableSingleton<AssetManager>.Instance.getAttractionObjects())
			{
				if (current.getUnlocalizedName() == name)
				{
					original = (TrackedRide)current;
					break;
				}
			}
			return original;
		}



		public static void PassMeshGeneratorProperties(MeshGenerator meshgeneratorFrom, MeshGenerator meshGeneratorTo)
		{
			meshGeneratorTo.stationPlatformGO = meshgeneratorFrom.stationPlatformGO;
			meshGeneratorTo.material = meshgeneratorFrom.material;
			meshGeneratorTo.liftMaterial = meshgeneratorFrom.liftMaterial;
			meshGeneratorTo.frictionWheelsGO = meshgeneratorFrom.frictionWheelsGO;
			meshGeneratorTo.supportInstantiator = meshgeneratorFrom.supportInstantiator;
			meshGeneratorTo.crossBeamGO = meshgeneratorFrom.crossBeamGO;
			meshGeneratorTo.customColors = meshgeneratorFrom.customColors;
		}
	}
}
