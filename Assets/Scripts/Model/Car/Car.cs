using DG.Tweening;
using Model.Paths;
using UnityEngine;
using Zenject;

namespace Model.Car
{
    public class Car : MonoBehaviour
    {
        [Inject] private PathsManager _pathsManager;
        
        [SerializeField] private float _maxSpeed = 5;
        [SerializeField] private float _acceleration = 1f;
        [SerializeField] private float _deceleration = 1f;
        
        private Path _currentPath;
        private Waypoint _currentWayPoint;
        private Waypoint _nextWayPoint;
        private Waypoint _destinationWayPoint;

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            var position = transform.position;
            
            _currentPath = _pathsManager.GetClosestPath(position);
            _currentWayPoint = _currentPath.GetClosestWaypoint(position);
            _currentPath.TryGetNextWaypoint(_currentWayPoint, out _nextWayPoint);
        }

        private float _currentSpeed;

        private void Update()
        {
            MoveTowardNextWayPoint();
            RotateTowardNextWayPoint();
        }

        private void MoveTowardNextWayPoint()
        {
            if (!_nextWayPoint) return;
            
            var destination = _nextWayPoint.transform.position;

            _currentSpeed = Mathf.Min(_maxSpeed, _currentSpeed + _acceleration * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, destination,
                _currentSpeed * Time.deltaTime);
            
            if (Vector3.Distance(transform.position, _nextWayPoint.transform.position) < 0.1f)
            {
                _currentWayPoint = _nextWayPoint;
                _currentPath.TryGetNextWaypoint(_currentWayPoint, out _nextWayPoint);
            }
        }
        
        private void RotateTowardNextWayPoint()
        {
            if (!_nextWayPoint) return;
            
            var destination = _nextWayPoint.transform.position;
            var direction = (destination - transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;   
            
            transform.DORotate(new Vector3(0, 0, angle), 0.5f);

        }
    }
}
