using System.Collections.Generic;
using UnityEngine;

namespace Model.Paths
{
    public class PathFinder
    {
        private const float DetectRadius = 1f;
        
        private readonly List<Path> _paths;
        
        public PathFinder(List<Path> paths)
        {
            _paths = paths;
        }

        public Path GetClosestPath(Vector2 position)
        {
            var closestPath = _paths[0];
            var closestDistance = float.MaxValue;

            foreach (var path in _paths)
            {
                foreach (var waypoint in path.Waypoints)
                {
                    var distance = Vector2.Distance(position, waypoint.transform.position);
                    if (!(distance < closestDistance)) continue;
                    
                    closestDistance = distance;
                    closestPath = path;
                }
            }

            return closestPath;
        }
        
        public List<Path> GetClosestPath(Waypoint currentWaypoint)
        {
            var closestPaths = new List<Path>();

            foreach (var path in _paths)
            {
                if (Vector2.Distance(currentWaypoint.Position, path.StartPoint.transform.position) < DetectRadius)
                    closestPaths.Add(path);
            }

            return closestPaths;
        }
        
        public bool ArePathsClose(Path path1, Path path2)
        {
            return Vector2.Distance(path1.EndPoint.transform.position, path2.StartPoint.transform.position) < DetectRadius;
        }
    }
}