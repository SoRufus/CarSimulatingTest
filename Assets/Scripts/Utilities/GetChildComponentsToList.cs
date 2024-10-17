using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class GetChildComponentsToList
    {
        public static List<T> Get<T>(GameObject obj)
        {
            List<T> list = new List<T>();
            
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
