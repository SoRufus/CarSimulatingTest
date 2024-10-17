using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Model.Paths
{
    public class PathsManager : MonoBehaviour
    {
        [SerializeField] private List<Path> _paths = new();

        [ContextMenu(nameof(GetPathsToList))]
        public void GetPathsToList()
        {
            _paths = GetChildComponentsToList.Get<Path>(gameObject);
        }

        public Path GetClosestPath(Vector2 position)
        {
            var closestPath = _paths[0];
            
            foreach (var path in _paths)
            {
                if (Vector2.Distance(position, path.transform.position) <
                    Vector2.Distance(position, closestPath.transform.position))
                {
                    closestPath = path;
                }
            }

            return closestPath;
        }
    }
}
