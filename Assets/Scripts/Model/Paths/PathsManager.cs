using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace Model.Paths
{
    public class PathsManager : MonoBehaviour
    {
        [SerializeField] private List<Path> _paths = new();
        
        private const float DetectRadius = 2f;

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
        
        public Waypoint GetShortestPath(Path startPath, List<Waypoint> waypoints)
        {
            Waypoint shortestWaypoint = null;
            var shortestDistance = float.MaxValue;

            foreach (var waypoint in waypoints)
            {
                var path = GetShortestRoadToWaypoint(startPath, waypoint);
                var distance = CalculatePathDistance(path);

                if (!(distance < shortestDistance)) continue;
                
                shortestDistance = distance;
                shortestWaypoint = waypoint;
            }

            return shortestWaypoint;
        }
        
        public List<Path> GetShortestRoadToWaypoint(Path startPath, Waypoint endWaypoint)
        {
            var priorityQueue = InitializePriorityQueue(startPath);
            var visited = new HashSet<Path>();

            while (priorityQueue.Count > 0)
            {
                var (currentPath, path) = DequeuePath(priorityQueue);

                if (currentPath.Waypoints.Contains(endWaypoint))
                    return path;

                ExploreNextPaths(currentPath, path, priorityQueue, visited);
            }

            return new List<Path>();
        }

        private SortedDictionary<float, List<(Path path, List<Path> paths)>> InitializePriorityQueue(Path startPath)
        {
            var priorityQueue = new SortedDictionary<float, List<(Path path, List<Path> paths)>>();

            foreach (var path in GetClosestPaths(startPath.EndPoint))
            {
                if (ArePathsNextToEachOther(startPath, path)) continue;

                if (!priorityQueue.ContainsKey(0))
                {
                    priorityQueue[0] = new List<(Path path, List<Path> paths)>();
                }
                priorityQueue[0].Add((path, new List<Path> { path }));
            }

            return priorityQueue;
        }

        private (Path currentPath, List<Path> path) DequeuePath(SortedDictionary<float, List<(Path path, List<Path> paths)>> priorityQueue)
        {
            var firstKey = priorityQueue.Keys.First();
            var pathTuple = priorityQueue[firstKey].First();
            priorityQueue[firstKey].RemoveAt(0);
            if (priorityQueue[firstKey].Count == 0)
            {
                priorityQueue.Remove(firstKey);
            }

            return pathTuple;
        }

        private void ExploreNextPaths(Path currentPath, List<Path> path, SortedDictionary<float, List<(Path path, List<Path> paths)>> priorityQueue, HashSet<Path> visited)
        {
            foreach (var waypoint in currentPath.Waypoints)
            {
                foreach (var nextPath in GetClosestPaths(waypoint))
                {
                    if (visited.Add(nextPath))
                    {
                        var newDistance = CalculatePathDistance(new List<Path> { currentPath, nextPath });
                        if (!priorityQueue.ContainsKey(newDistance))
                        {
                            priorityQueue[newDistance] = new List<(Path path, List<Path> paths)>();
                        }
                        priorityQueue[newDistance].Add((nextPath, new List<Path>(path) { nextPath }));
                    }
                }
            }
        }

        private float CalculatePathDistance(List<Path> paths)
        {
            var totalDistance = 0f;

            for (int i = 0; i < paths.Count - 1; i++)
            {
                totalDistance += Vector2.Distance(paths[i].EndPoint.transform.position, 
                    paths[i + 1].StartPoint.transform.position);
            }

            return totalDistance;
        }
        
        private List<Path> GetClosestPaths(Waypoint currentWaypoint)
        {
            var closestPaths = new List<Path>();

            foreach (var path in _paths)
            {
                if (Vector2.Distance(currentWaypoint.Position, path.StartPoint.transform.position) < DetectRadius)
                    closestPaths.Add(path);
            }

            return closestPaths;
        }
        
        private bool ArePathsNextToEachOther(Path path1, Path path2, float detectRadius = 1f)
        {
            var distance = Vector2.Distance(path1.EndPoint.transform.position, path2.StartPoint.transform.position);
            if (distance > detectRadius)
            {
                return false;
            }
            
            var distance2 = Vector2.Distance(path1.StartPoint.transform.position, path2.EndPoint.transform.position);
            if (distance2 > detectRadius)
            {
                return false;
            }

            return true;
        }
    }
}
