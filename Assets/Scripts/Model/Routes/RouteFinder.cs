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
        
        private const float DetectRadius = 2f;

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
                    return new RouteData(route, CalculateRouteLength(route));
                }

                ExploreNextPaths(currentPath, route, priorityQueue, visited);
            }

            return default;
        }

        private SortedDictionary<float, List<(Path path, List<Path> paths)>> InitializePriorityQueue(Path startPath)
        {
            var priorityQueue = new SortedDictionary<float, List<(Path path, List<Path> paths)>>();

            foreach (var path in _pathFinder.GetClosestPath(startPath.EndPoint))
            {
                AddToPriorityQueue(priorityQueue, path.Length, path, new List<Path> {startPath, path });
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

        private void ExploreNextPaths(Path currentPath, List<Path> path,
            IDictionary<float, List<(Path path, List<Path> paths)>> priorityQueue, ISet<Path> visited)
        {
            foreach (var waypoint in currentPath.Waypoints)
            {
                foreach (var nextPath in _pathFinder.GetClosestPath(waypoint))
                {
                    if (!visited.Add(nextPath)) continue;

                    var newDistance = CalculateRouteLength(new List<Path> { currentPath, nextPath });
                    AddToPriorityQueue(priorityQueue, newDistance, nextPath, new List<Path>(path) { nextPath });
                }
            }
        }

        private void AddToPriorityQueue(IDictionary<float, List<(Path path, List<Path> paths)>> priorityQueue, float distance, Path path, List<Path> paths)
        {
            if (!priorityQueue.ContainsKey(distance))
            {
                priorityQueue[distance] = new List<(Path path, List<Path> paths)>();
            }
            priorityQueue[distance].Add((path, paths));
        }

        private float CalculateRouteLength(IEnumerable<Path> paths)
        {
            return paths.Sum(path => path.Length);
        }

        // private bool ArePathsNextToEachOther(Path path1, Path path2)
        // {
        //     var distance = Vector2.Distance(path1.EndPoint.transform.position, path2.StartPoint.transform.position);
        //     var distance2 = Vector2.Distance(path1.StartPoint.transform.position, path2.EndPoint.transform.position);
        //
        //     return distance <= DetectRadius && distance2 <= DetectRadius;
        // }
    }
}