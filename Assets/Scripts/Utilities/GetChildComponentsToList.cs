using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class GetChildComponentsToList
    {
        // Get all components of type T from children of obj, and return them in a list
        public static List<T> Get<T>(GameObject obj)
        {
            var list = new List<T>();
            
            foreach (Transform child in obj.transform)
            {
                var component = child.GetComponent<T>();
                if (component != null)
                {
                    list.Add(component);
                }
            }

            return list;
        }
    }
}
