using System.Collections.Generic;
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
        [SerializeField] private float _rotationMultiplier = 1f;
        [SerializeField] private float _maxRotationDuration = 1f;


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
            RotateTowardNextWayPoint(0);
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
            
            _currentSpeed = _nextWayPoint == _destinationWayPoint || _currentPath.StartPoint == _nextWayPoint ?
                Mathf.Max(0, _currentSpeed - _deceleration * Time.deltaTime) :
                Mathf.Min(_maxSpeed, _currentSpeed + _acceleration * Time.deltaTime);

            transform.position = Vector2.MoveTowards(transform.position, destination,
                _currentSpeed * Time.deltaTime);
        }

        private void GetNextWayPoint()
        {
            _currentWayPoint = _nextWayPoint;
            
            if (!_currentPath.TryGetNextWaypoint(_currentWayPoint, out _nextWayPoint))
            {
                ChangePath();
            }
            
            RotateTowardNextWayPoint((Vector2.Distance(_currentWayPoint.Position, 
                _nextWayPoint.Position) / _currentSpeed) * _rotationMultiplier);
            
            if (_currentWayPoint == _destinationWayPoint)
            {
                ReachedDestination();
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

        private void RotateTowardNextWayPoint(float duration)
        {
            if (!_nextWayPoint) return;
            
            var destination = _nextWayPoint.transform.position;
            var direction = (destination - transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;   
            
            transform.DORotate(new Vector3(0, 0, angle), Mathf.Min(_maxRotationDuration, duration));

        }
        public Path CurrentPath => _currentPath;
    }
}
