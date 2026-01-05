using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        [SerializeField] private List<SerializableKeyValuePair> _items = new List<SerializableKeyValuePair>();

        private Dictionary<TKey, TValue> _dictionary;

        [Serializable]
        private class SerializableKeyValuePair
        {
            public TKey Key;
            public TValue Value;
        }

        public void Initialize()
        {
            _dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in _items)
            {
                if (!_dictionary.ContainsKey(item.Key))
                {
                    _dictionary.Add(item.Key, item.Value);
                }
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_dictionary == null)
            {
                Initialize();
            }

            return _dictionary.TryGetValue(key, out value);
        }

        public Dictionary<TKey, TValue> GetDictionary()
        {
            if (_dictionary == null)
            {
                Initialize();
            }

            return _dictionary;
        }
    }
}