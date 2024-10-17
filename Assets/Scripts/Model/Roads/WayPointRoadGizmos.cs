namespace Model.Roads
{
    using UnityEngine;

    namespace Model
    {
        [RequireComponent(typeof(WayPointRoad))]
        public class WayPointRoadGizmos : MonoBehaviour
        {
            [SerializeField] private Color _color = Color.white;
            [SerializeField] private WayPointRoad _wayPointRoad;

            private void OnDrawGizmos()
            {
                Gizmos.color = _color;
                
                var waypoints = _wayPointRoad.Waypoints;
                
                if (_wayPointRoad == null || waypoints.Count < 2)
                {
                    return;
                }
                
                for (int i = 0; i < waypoints.Count - 1; i++)
                {
                    Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
                }
            }
        }
    }
}