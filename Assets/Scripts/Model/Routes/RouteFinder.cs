using System.Collections.Generic;
using System.Linq;
using Model.Paths;
using UnityEngine;
using Zenject;

namespace Model.Routes
{
    public class RouteFinder
    {
        [Inject] private readonly PathFinder _pathFinder;

        private const float MaxAngle = 120;
        
        public Waypoint GetWaypointWithShortestRouteFromPath(Path startPath, List<Waypoint> waypoints)
        {
            Waypoint closestWaypoint = null;
            var shortestDistance = float.MaxValue;

            foreach (var waypoint in waypoints)
            {
                var data = GetShortestRouteToWaypoint(startPath, waypoint);

                if (data.Distance < shortestDistance)
                {
                    shortestDistance = data.Distance;
                    closestWaypoint = waypoint;
                }
            }

            return closestWaypoint;
        }

        public RouteData GetShortestRouteToWaypoint(Path startPath, Waypoint endWaypoint)
        {
            var priorityQueue = InitializePriorityQueue(startPath);

            var visited = new HashSet<Path>();

            while (priorityQueue.Count > 0)
            {
                var (currentPath, route) = DequeuePath(priorityQueue);

                if (currentPath.Waypoints.Contains(endWaypoint))
                {
                    return new RouteData(route.Paths, CalculateRouteLength(route.Paths));
                }

                ExploreNextPaths(currentPath, route.Paths, priorityQueue, visited);
            }

            Debug.Log($"No route found from {startPath.StartPoint} to {endWaypoint}");
            return default;
        }

        private SortedDictionary<float, List<(Path path, RouteData data)>> InitializePriorityQueue(Path startPath)
        {
            var priorityQueue = new SortedDictionary<float, List<(Path path, RouteData data)>>();

            foreach (var path in _pathFinder.GetClosestPath(startPath.EndPoint))
            {
                if (GetPathsAngle(startPath, path) > MaxAngle) continue;
                AddToPriorityQueue(priorityQueue, path, new RouteData(new List<Path>(){startPath, path}, path.Length));
            }

            return priorityQueue;
        }

        private (Path currentPath, RouteData data) DequeuePath(SortedDictionary<float, List<(Path path, RouteData data)>> priorityQueue)
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

        private void ExploreNextPaths(Path currentPath, List<Path> path,
            IDictionary<float, List<(Path path, RouteData data)>> priorityQueue, ISet<Path> visited)
        {
            foreach (var waypoint in currentPath.Waypoints)
            {
                foreach (var nextPath in _pathFinder.GetClosestPath(waypoint))
                {
                    if (GetPathsAngle(currentPath, nextPath) > MaxAngle) continue;
                    if (!_pathFinder.ArePathsClose(currentPath, nextPath)) continue;

                    if (!visited.Add(nextPath)) continue;
                    
                    var newPathList = new List<Path>(path) { nextPath };
                    var newData = new RouteData(newPathList, CalculateRouteLength(newPathList));

                    AddToPriorityQueue(priorityQueue, nextPath, newData);
                }
            }
        }

        private void AddToPriorityQueue(IDictionary<float, List<(Path path, RouteData data)>> priorityQueue, Path path, RouteData data)
        {
            if (!priorityQueue.ContainsKey(data.Distance))
            {
                priorityQueue[data.Distance] = new List<(Path path, RouteData data)>();
            }

            priorityQueue[data.Distance].Add((path, data));
        }

        private float CalculateRouteLength(IEnumerable<Path> paths)
        {
            return paths.Sum(path => path.Length);
        }
        
        private float GetPathsAngle(Path path1, Path path2)
        {
            var path1Direction = (path1.EndPoint.Position - path1.GetPreviousWaypoint(path1.EndPoint).Position).normalized;
            var path2Direction = (path2.GetNextWayPoint(path2.StartPoint).Position - path2.StartPoint.Position).normalized;
            return Vector3.Angle(path1Direction, path2Direction);
        }
    }
}