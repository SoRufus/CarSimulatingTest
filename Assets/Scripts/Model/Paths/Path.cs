using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Model.Paths
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> _waypoints = new();

        private float _length;

        [ContextMenu(nameof(GetWayPointsToList))]
        public void GetWayPointsToList()
        {
            _waypoints = GetChildComponentsToList.Get<Waypoint>(gameObject);
        }

        private void OnEnable()
        {
            CalculatePathLength();
        }

        private void CalculatePathLength()
        {
            for (var i = 0; i < _waypoints.Count - 1; i++)
            {
                _length += Vector2.Distance(_waypoints[i].transform.position, _waypoints[i + 1].transform.position);
            }
        }

        public Waypoint GetClosestWaypoint(Vector2 position)
        {
            var closestWaypoint = _waypoints[0];
            
            foreach (var waypoint in _waypoints)
            {
                if (Vector2.Distance(position, waypoint.transform.position) <
                    Vector2.Distance(position, closestWaypoint.transform.position))
                {
                    closestWaypoint = waypoint;
                }
            }

            return closestWaypoint;
        }
        
        public bool TryGetNextWaypoint(Waypoint currentWaypoint, out Waypoint waypoint)
        {
            var indexOfCurrentWaypoint = _waypoints.IndexOf(currentWaypoint);
            var indexOfNextWaypoint = indexOfCurrentWaypoint + 1;
            waypoint = null;
            
            if (indexOfNextWaypoint > _waypoints.Count - 1)
            {
                return false;
            }

            waypoint = _waypoints[indexOfNextWaypoint];
            return true;
        }
        
        public Waypoint GetNextWayPoint(Waypoint currentWaypoint)
        {
            var indexOfCurrentWaypoint = _waypoints.IndexOf(currentWaypoint);

            return _waypoints[indexOfCurrentWaypoint + 1];
        }
        
        public Waypoint GetPreviousWaypoint(Waypoint currentWaypoint)
        {
            var indexOfCurrentWaypoint = _waypoints.IndexOf(currentWaypoint);

            return _waypoints[indexOfCurrentWaypoint - 1];
        }

        public List<Waypoint> Waypoints => _waypoints;
        public Waypoint StartPoint => _waypoints[0];
        public Waypoint EndPoint => _waypoints[^1];
        public float Length => _length;
    }
}
