using System.Collections.Generic;
using Model.Paths;
using UnityEngine;
using Zenject;

namespace Model.Spots
{
    public class Spot : MonoBehaviour
    {
        [Inject] private readonly Car.Car _car;
        [Inject] private readonly PathsManager _manager;

        [SerializeField] private List<Waypoint> _waypoints = new();

        public void OnClick()
        {
            var currentPath = _car.CurrentPath;
            var nearestWayPoint = _manager.GetShortestPath(currentPath, _waypoints);
            var paths = _manager.GetShortestRoadToWaypoint(currentPath, nearestWayPoint);

            _car.SetPathsToDestination(paths, nearestWayPoint);
        }
    }
}
