using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Metelab
{
    public class MeteSingletonScriptableObject<T> : MeteScriptableObject where T : MeteSingletonScriptableObject<T> 
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                }
                return instance;
            }
        }

        public override void EarlyInit()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
        }
    }

}
