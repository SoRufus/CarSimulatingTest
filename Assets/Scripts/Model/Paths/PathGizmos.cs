using UnityEngine;
using Zenject;

#if UNITY_EDITOR
namespace Model.Paths
{
    [RequireComponent(typeof(Path))]
    public class PathGizmos : MonoBehaviour
    {
        [Inject] private readonly Car.Car _car;
        
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private Color _selectedInEditorColor = Color.red;
        [SerializeField] private Color _carOnPathColor = Color.blue;
        [SerializeField] private Color _selectedRouteColor = Color.green;
        [SerializeField] private Path _path;

        private void OnDrawGizmos()
        {
            if (_car)
            {
                if (_car.Navigation.CurrentPath == _path)
                {
                    Gizmos.color = _car.Navigation.IsNextPositionDestination ? _selectedRouteColor : _carOnPathColor;
                }
                else if (_car.Navigation.CurrentRoute.Distance != 0 && _car.Navigation.CurrentRoute.Paths.Contains(_path))
                {
                    Gizmos.color = _selectedRouteColor;
                }
                else
                {
                    Gizmos.color = _color;
                }
            }
            
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _selectedInEditorColor;
            
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
#endif