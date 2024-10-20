using System.Collections.Generic;
using Model.Routes;
using UnityEngine;
using Zenject;

namespace Model.Spots
{
    
    public class Spot : MonoBehaviour
    {
        [Inject] private readonly Car.Car _car;
        [Inject] private readonly RouteFinder _routeFinder;

        [SerializeField] private List<Waypoint> _waypoints = new();

        public void OnClick()
        {
            var currentPath = _car.Navigation.CurrentPath;
            var nearestWayPoint = _routeFinder.GetWaypointWithShortestRouteFromPath(currentPath, _waypoints);
            var data = _routeFinder.GetShortestRouteToWaypoint(currentPath, nearestWayPoint);
            if (data.Distance == 0) return;

            _car.Navigation.SetRouteToDestination(data, nearestWayPoint);
        }
    }
}
