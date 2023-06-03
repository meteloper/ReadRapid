using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Metelab
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField]
        private List<TKey> keys = new List<TKey>();
        [SerializeField]
        private List<TValue> values = new List<TValue>();
#endif

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            UpdateInspector();
#endif
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            UpdateInspector();
#endif
        }

        public void UpdateInspector()
        {
#if UNITY_EDITOR
            keys = Keys.ToList();
            values = Values.ToList();
#endif
        }

    }

}
