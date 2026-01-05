using System.Collections.Generic;
using Enums;
using Extensions;
using UnityEngine;
using Zenject;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PrefabContainer", menuName = "ScriptableObjects/Prefab/PrefabContainer")]
    public class PrefabContainer : ScriptableObject
    {
        [SerializeField] private List<PrefabList> _prefabLists;

        private DiContainer _diContainer;

        public void Initialize(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public List<GameObject> GetListPrefabOfType(PrefabType type)
        {
            var typeName = type.ToString();
            List<GameObject> prefabs = new List<GameObject>();

            foreach (var prefabList in _prefabLists)
            {
                if (prefabList.Type == typeName)
                {
                    foreach (var prefab in prefabList.Prefabs)
                    {
                        prefabs.Add(prefab);
                    }
                }
            }

            return prefabs;
        }

        public GameObject GetObjectOfType<T>(PrefabType type, string name) where T : Component
        {
            List<GameObject> prefabs = GetListPrefabOfType(type);
            return prefabs.FirstObjectAs<T>(name);
        }

        public GameObject InstantiatePrefab(PrefabType type, string prefabName, Transform parent = null)
        {
            if (_diContainer == null)
            {
                Debug.LogError("PrefabContainer not initialized with DiContainer. Call Initialize() first.");
                return null;
            }

            GameObject prefab = FindPrefab(type, prefabName);
            if (prefab == null)
            {
                Debug.LogError($"Prefab '{prefabName}' of type '{type}' not found in PrefabContainer.");
                return null;
            }

            GameObject instance = _diContainer.InstantiatePrefab(prefab, parent);
            return instance;
        }

        public T InstantiatePrefab<T>(PrefabType type, string prefabName, Transform parent = null) where T : Component
        {
            GameObject instance = InstantiatePrefab(type, prefabName, parent);
            if (instance == null)
                return null;

            T component = instance.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"Prefab '{prefabName}' does not have component of type {typeof(T).Name}");
            }

            return component;
        }

        private GameObject FindPrefab(PrefabType type, string prefabName)
        {
            var typeName = type.ToString();

            foreach (var prefabList in _prefabLists)
            {
                if (prefabList.Type == typeName)
                {
                    foreach (var prefab in prefabList.Prefabs)
                    {
                        if (prefab.name == prefabName)
                            return prefab;
                    }
                }
            }

            return null;
        }
    }
}