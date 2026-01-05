using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public class PrefabList
    {
        [SerializeField] public string Type;
        [SerializeField] public List<GameObject> Prefabs;
    }
}