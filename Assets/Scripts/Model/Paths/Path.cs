using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Model.Paths
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> _waypoints = new();

        [ContextMenu(nameof(GetWayPointsToList))]
        public void GetWayPointsToList()
        {
            _waypoints = GetChildComponentsToList.Get<Waypoint>(gameObject);
        }
        
        public Waypoint GetClosestWaypoint(Vector2 position)
        {
            var closestWaypoint = _waypoints[0];
            
            foreach (var path in _waypoints)
            {
                if (Vector2.Distance(position, path.transform.position) <
                    Vector2.Distance(position, closestWaypoint.transform.position))
                {
                    closestWaypoint = path;
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

        public List<Waypoint> Waypoints => _waypoints;
        public Waypoint StartPoint => _waypoints[0];
        public Waypoint EndPoint => _waypoints[^1];
    }
}
