using UnityEngine;
using UnityEngine.Serialization;

namespace Model.Paths
{
    namespace Model
    {
        [RequireComponent(typeof(Path))]
        public class PathGizmos : MonoBehaviour
        {
            [SerializeField] private Color _color = Color.white;
            [SerializeField] private Path _path;

            private void OnDrawGizmos()
            {
                Gizmos.color = _color;
                
                var waypoints = _path.Waypoints;
                
                if (_path == null || waypoints.Count < 2)
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