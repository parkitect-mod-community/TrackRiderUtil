using System;
using System.Collections.Generic;
using UnityEngine;

namespace TrackedRiderUtility
{
    public class CoasterDepender
    {
        private TrackedRide trackedRide;
        private List<CoasterCarInstantiator> coasterCarInstantiator = new List<CoasterCarInstantiator>();
       private string hash;
        private List<UnityEngine.Object> registeredObjects = new List<UnityEngine.Object>();

        private GameObject hider;

        public CoasterDepender(TrackedRide ride,string name, string hash,string displayName)
        {
            hider = new GameObject ();
            this.hash = hash;
            this.trackedRide = ride;
            this.trackedRide.name = name + hash;
        }

        public CoasterDepender(string unlocalizedName,string name, string hash,string DisplayName)
        {

            hider = new GameObject ();
            this.hash = hash;
            this.trackedRide = CoasterHelper.GetTrackedRide(unlocalizedName);
            this.trackedRide.name = name + hash;

        }


        public MeshGenerator MeshGenerator{
            get{ return trackedRide.meshGenerator; }
            set{this.trackedRide.meshGenerator = value;}
        }

        public TrackedRide TrackedRide {
            get{ return this.trackedRide; }
            set{ this.trackedRide = value; }
        }

        public T GetInstaniator<T>(String name,String displayName,int defaultTrain, int maxCarts, int minCarts) where T : CoasterCarInstantiator
        {
            T instaniator = ScriptableObject.CreateInstance<T> ();

            instaniator.displayName = displayName;
            instaniator.maxTrainLength = maxCarts;
            instaniator.minTrainLength = minCarts;
            instaniator.name = name + hash;

            coasterCarInstantiator.Add (instaniator);
            RegisterToAssetManager (instaniator);
            this.trackedRide.carTypes = coasterCarInstantiator.ToArray ();
            return instaniator;
        }

        private void RegisterToAssetManager(UnityEngine.Object obj)
        {
            registeredObjects.Add (obj);
            AssetManager.Instance.registerObject (obj);

        }

        public T RegisterCar<T>(GameObject cart,string name, float offsetBack, float offsetFront,bool front) where T : BaseCar 
        {
            GameObject cartGo = UnityEngine.GameObject.Instantiate(cart);
            T item = cartGo.AddComponent<T> ();
            item.name = name + hash;
            item.offsetBack = offsetBack;
            item.offsetFront = offsetFront;

            item.Decorate (front);

            Rigidbody rigidBody =  cart.GetComponent<Rigidbody>();
            if (rigidBody == null)
                rigidBody = cart.AddComponent<Rigidbody> ();
            rigidBody.isKinematic = true;

            BoxCollider boxCollider =  cart.GetComponent<BoxCollider>();
            if (boxCollider == null)
                boxCollider = cart.AddComponent<BoxCollider> ();

            this.RegisterToAssetManager (cartGo);
            cartGo.transform.SetParent (hider.transform);


            return item;
        }

        public void Apply()
        {
            hider.SetActive (false);
        }

        public void Unload()
        {

            foreach(UnityEngine.Object o in registeredObjects)
            {
                AssetManager.Instance.unregisterObject (o);
            }
            UnityEngine.GameObject.DestroyImmediate (hider);
     
        }


  

    }
}

