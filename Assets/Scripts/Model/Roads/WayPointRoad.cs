using System.Collections.Generic;
using UnityEngine;

namespace Model.Roads
{
    public class WayPointRoad : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> _waypoints = new();

        [ContextMenu(nameof(GetWayPointsToList))]
        public void GetWayPointsToList()
        {
            _waypoints = new List<Waypoint>();
            
            foreach (Transform child in gameObject.transform)
            {
                var waypoint = child.GetComponent<Waypoint>();
                if (waypoint != null)
                {
                    _waypoints.Add(waypoint);
                }
            }
        }

        public List<Waypoint> Waypoints => _waypoints;
    }
}
