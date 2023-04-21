using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jacobs.Core
{
     /**
     *  Manages static instances where the defined generic type inherits Unity's component class.
     * 
     *  @owner Thomas Jacobs.
     *  @date 3/4/23.
     */
    public class MonoSingleton<DerivedType> : MonoBehaviour where DerivedType : Component
    {
        //Attributes:
        private static DerivedType singleton_instance;
        private const uint SINGLETON_INSTANCE_INDEX = 0;

        //Methods:

        /**
         *  @return Singleton instance of the defined, generic type.
         */
        public static DerivedType Singleton
        {
            get
            {
                if (singleton_instance != null) return singleton_instance;

                DerivedType[] obj = FindObjectsOfType<DerivedType>();

                if (obj.Length > 0)
                {
                    if (obj.Length > 1) { Debug.LogError("There is more than one " + typeof(DerivedType).Name + " instance in the scene."); }
                    singleton_instance = obj[SINGLETON_INSTANCE_INDEX]; 
                    return singleton_instance;
                }

                //If no instance exists, create and initialise a new instance.
                singleton_instance = new GameObject(typeof(DerivedType).Name + "_Singleton").AddComponent<DerivedType>();
                return singleton_instance;
            }
        }
    }
}