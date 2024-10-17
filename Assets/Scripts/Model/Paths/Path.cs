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

        public List<Waypoint> Waypoints => _waypoints;
    }
}
