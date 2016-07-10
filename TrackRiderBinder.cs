using System;
using System.Collections.Generic;
using UnityEngine;

namespace TrackedRiderUtility
{
	public class TrackRiderBinder
	{
		private List<UnityEngine.Object> registeredObjects = new List<UnityEngine.Object>();

		private string hash;

		private GameObject hider;

		public TrackRiderBinder(string hash)
		{
			this.hider = new GameObject();
			this.hash = hash;
		}

		private string GetNameHash(string name)
		{
			return name + "-" + this.hash;
		}

		public T RegisterTrackedRide<T>(string name, string displayName) where T : TrackedRide
		{
			GameObject gameObject = new GameObject();
			T result = gameObject.AddComponent<T>();
			result.name = this.GetNameHash(name);
			result.setDisplayName(displayName);
			this.registeredObjects.Add(gameObject);
			return result;
		}

		public T RegisterTrackedRide<T>(string unlocalizedName, string name, string displayName) where T : TrackedRide
		{
			T t = (T)((object)UnityEngine.Object.Instantiate<TrackedRide>(TrackRideHelper.GetTrackedRide(unlocalizedName)));
			t.name = this.GetNameHash(name);
			t.setDisplayName(displayName);
			this.registeredObjects.Add(t);
			return t;
		}

		public T RegisterSupportGenerator<T>(TrackedRide assignedto) where T : SupportInstantiator
		{
			T t = ScriptableObject.CreateInstance<T>();
			this.registeredObjects.Add(t);
			assignedto.meshGenerator.supportInstantiator = t;
			return t;
		}

		public T RegisterMeshGenerator<T>(TrackedRide assignedto) where T : MeshGenerator
		{
			T t = ScriptableObject.CreateInstance<T>();
			this.registeredObjects.Add(t);
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
			t.name = this.GetNameHash(name);
			this.registeredObjects.Add(t);
			Array.Resize<CoasterCarInstantiator>(ref assignedto.carTypes, assignedto.carTypes.Length + 1);
			assignedto.carTypes[assignedto.carTypes.Length - 1] = t;
			return t;
		}

        public T RegisterCar<T>(GameObject cart, string name, float offsetBack, float offsetFront, bool front, Color[] colors) where T : Car
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(cart);
			T t = gameObject.AddComponent<T>();
			t.name = this.GetNameHash(name);
			t.offsetBack = offsetBack;
			t.offsetFront = offsetFront;
			this.registeredObjects.Add(t);
            if(t is BaseCar)
                (t as BaseCar).Decorate(front);
			Rigidbody rigidbody = cart.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = cart.AddComponent<Rigidbody>();
			}
			rigidbody.isKinematic = true;
			BoxCollider x = cart.GetComponent<BoxCollider>();
			if (x == null)
			{
				x = cart.AddComponent<BoxCollider>();
			}
			gameObject.transform.SetParent(this.hider.transform);
			TrackRideHelper.MakeRecolorble(gameObject, "CustomColorsDiffuse", colors);
			return t;
		}

		public void Apply()
		{
			for (int i = 0; i < this.registeredObjects.Count; i++)
			{
				ScriptableSingleton<AssetManager>.Instance.registerObject(this.registeredObjects[i]);
			}
			this.hider.SetActive(false);
		}

		public void Unload()
		{
			foreach (UnityEngine.Object current in this.registeredObjects)
			{
				ScriptableSingleton<AssetManager>.Instance.unregisterObject(current);
			}
			UnityEngine.Object.DestroyImmediate(this.hider);
		}
	}
}
