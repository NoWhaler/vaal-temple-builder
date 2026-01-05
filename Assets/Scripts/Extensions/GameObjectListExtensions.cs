using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class GameObjectListExtensions
    {
        public static GameObject FirstObjectAs<T>(this List<GameObject> list, string name) where T : Component
        {
            foreach (var gameObject in list)
            {
                if (gameObject.name == name && gameObject.GetComponent<T>() != null)
                {
                    return gameObject;
                }
            }

            return null;
        }
    }
}