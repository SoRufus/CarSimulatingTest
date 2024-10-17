using System.Collections.Generic;
using Model;
using Model.Car;
using Model.Paths;
using UnityEngine;
using Zenject;

public class Spot : MonoBehaviour
{
    [Inject] private readonly Car _car;
    [Inject] private readonly PathsManager _manager;

    [SerializeField] private List<Waypoint> _waypoints = new();

    public void OnClick()
    {
        var startingPath = _manager.GetClosestPath(_car.transform.position);
        var nearestWayPoint = _manager.GetShortestPath(startingPath, _waypoints);
        var paths = _manager.GetShortestRoadToWaypoint(startingPath, nearestWayPoint);

        _car.SetPathsToDestination(paths, nearestWayPoint);
    }
}
