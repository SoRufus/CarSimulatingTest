using System.Collections.Generic;
using Model.Paths;
using Model.Stats;
using UnityEngine;
using Zenject;

namespace Model.Car
{
    public class Car : MonoBehaviour
    {
        [Inject] private PathsManager _pathsManager;

        [SerializeField] private StatsConfig _statsConfig;
        [SerializeField] private float _rotationMultiplier = 1f;
        
        private float _currentSpeed;
        
        private List<Path> _pathsToDestination = new();
        private Path _currentPath;
        private int _currentPathIndex;
        
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

        public void SetPathsToDestination(List<Path> paths, Waypoint destination)
        {
            _pathsToDestination.Clear();
            var newRoad = new List<Path> { _currentPath };
            newRoad.AddRange(paths);
            
            _currentPathIndex = 0;
            _pathsToDestination = newRoad;
            _destinationWayPoint = destination;
        }
        
        private void Update()
        {
            MoveTowardNextWayPoint();
            RotateTowardNextWayPoint();

        }

        private void MoveTowardNextWayPoint()
        {
            if (!_nextWayPoint) return;
            if (!_destinationWayPoint) return;
            if (_pathsToDestination.Count == 0) return;

            var destination = _nextWayPoint.transform.position;

            if (Vector3.Distance(transform.position, _nextWayPoint.transform.position) < 0.1f)
            {
                GetNextWayPoint();
            }
            
            _currentSpeed = _nextWayPoint == _destinationWayPoint ?
                Mathf.Max(0, _currentSpeed - _statsConfig.Deceleration.Value * _currentSpeed * Time.deltaTime) :
                Mathf.Min(_statsConfig.MaxSpeed.Value, _currentSpeed + _statsConfig.Acceleration.Value * Time.deltaTime);

            transform.position = Vector2.MoveTowards(transform.position, destination,
                _currentSpeed * Time.deltaTime);
        }

        private void GetNextWayPoint()
        {
            _currentWayPoint = _nextWayPoint;
            
            if (_currentWayPoint == _destinationWayPoint)
            {
                ReachedDestination();
                return;
            }
            
            if (!_currentPath.TryGetNextWaypoint(_currentWayPoint, out _nextWayPoint))
            {
                ChangePath();
            }
        }

        private void ReachedDestination()
        {
            _destinationWayPoint = null;
            _currentSpeed = 0;
        }

        private void ChangePath()
        {
            _currentPathIndex++;
            _currentPath = _pathsToDestination[_currentPathIndex];
            _nextWayPoint = _currentPath.StartPoint;
        }

        private void RotateTowardNextWayPoint() 
        {
            if (!_nextWayPoint) return;
            
            var destination = _nextWayPoint.transform.position;
            var direction = (destination - transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            var currentAngle = transform.rotation.eulerAngles.z;
            var rotationAmount = Mathf.Abs(Mathf.DeltaAngle(currentAngle, angle));

            if (_currentSpeed > _statsConfig.MaxSpeed.Value / 2)
            {
                _currentSpeed = Mathf.Max(0, _currentSpeed - _statsConfig.DecelerationOnTurns.Value * rotationAmount * 
                    _currentSpeed * Time.deltaTime);
            }

            var targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
                _rotationMultiplier * _currentSpeed * Time.deltaTime);

        }
        public Path CurrentPath => _currentPath;
        public StatsConfig StatsConfig => _statsConfig;
    }
}
