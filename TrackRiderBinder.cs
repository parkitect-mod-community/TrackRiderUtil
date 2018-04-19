using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TrackedRiderUtility
{
	public class TrackRiderBinder
	{
		private List<Object> registeredObjects = new List<Object>();

		private string hash;

		private GameObject hider;

		public TrackRiderBinder(string hash)
		{
			hider = new GameObject();
			this.hash = hash;
		}

		private string GetNameHash(string name)
		{
			return name + "-" + hash;
		}

		public T RegisterTrackedRide<T>(string name, string displayName) where T : TrackedRide
		{
			GameObject gameObject = new GameObject();
			T result = gameObject.AddComponent<T>();
			result.name = GetNameHash(name);
			result.setDisplayName(displayName);
			registeredObjects.Add(gameObject);
			return result;
		}

		public T RegisterTrackedRide<T>(string unlocalizedName, string name, string displayName) where T : TrackedRide
		{
			T t = (T)Object.Instantiate(TrackRideHelper.GetTrackedRide(unlocalizedName));
			t.name = GetNameHash(name);
			t.setDisplayName(displayName);
			registeredObjects.Add(t);
			return t;
		}

		public T RegisterSupportGenerator<T>(TrackedRide assignedto) where T : SupportInstantiator
		{
			T t = ScriptableObject.CreateInstance<T>();
			registeredObjects.Add(t);
			assignedto.meshGenerator.supportInstantiator = t;
			return t;
		}

		public T RegisterMeshGenerator<T>(TrackedRide assignedto) where T : MeshGenerator
		{
			T t = ScriptableObject.CreateInstance<T>();
			registeredObjects.Add(t);
			assignedto.meshGenerator = t;
			return t;
		}

		public T RegisterCoasterCarInstaniator<T>(TrackedRide assignedto, string name, string displayName, int defaultTrain, int maxCarts, int minCarts) where T : CoasterCarInstantiator
		{
			T t = ScriptableObject.CreateInstance<T>();
			t.displayName = displayName;
            t.defaultTrainLength = defaultTrain;
			t.maxTrainLength = maxCarts;
			t.minTrainLength = minCarts;
			t.name = GetNameHash(name);
			registeredObjects.Add(t);
			Array.Resize(ref assignedto.carTypes, assignedto.carTypes.Length + 1);
			assignedto.carTypes[assignedto.carTypes.Length - 1] = t;
			return t;
		}

        public T RegisterCar<T>(GameObject cart, string name, float offsetBack, float offsetFront, bool front, Color[] colors) where T : Car
		{
			GameObject gameObject = Object.Instantiate(cart);
			T t = gameObject.AddComponent<T>();
			t.name = GetNameHash(name);
			t.offsetBack = offsetBack;
			t.offsetFront = offsetFront;
			registeredObjects.Add(t);
            if(t is BaseCar)
                (t as BaseCar).Decorate(front);
			Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = gameObject.AddComponent<Rigidbody>();
			}
			rigidbody.isKinematic = true;
			BoxCollider collider = gameObject.GetComponent<BoxCollider>();
			if (collider == null)
			{
				gameObject.AddComponent<BoxCollider>();
			}

			gameObject.transform.SetParent(hider.transform);
			TrackRideHelper.MakeRecolorble(gameObject, "CustomColorsDiffuse", colors);
			return t;
		}

		public void Apply()
		{
			for (int i = 0; i < registeredObjects.Count; i++)
			{
				ScriptableSingleton<AssetManager>.Instance.registerObject(registeredObjects[i]);
			}
			hider.SetActive(false);
		}

		public void Unload()
		{
			foreach (Object current in registeredObjects)
			{
				ScriptableSingleton<AssetManager>.Instance.unregisterObject(current);
			}
			Object.DestroyImmediate(hider);
		}
	}
}
