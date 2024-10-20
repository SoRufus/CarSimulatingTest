using Model.Car.States;
using Model.Paths;
using Model.Routes;
using UnityEngine;
using Zenject;

namespace Model.Car.Components
{
    public class CarNavigation : CarComponent
    {
        [Inject] private readonly PathFinder _pathFinder;
        public Path CurrentPath { get; private set; }
        
        private const float WaypointDetectionDistance = 0.05f;

        private Waypoint _currentWayPoint;
        private Waypoint _nextWayPoint;
        private Waypoint _destinationWayPoint;
        
        private RouteData _currentRoute;
        private int _currentPathIndex;

        protected override void OnEnable()
        {
            base.OnEnable();
            Init();
        }

        public void UpdateNavigation()
        {
            DetectPoints();
        }

        private void Init()
        {
            var position = Car.Position;

            CurrentPath = _pathFinder.GetClosestPath(position);
            _currentWayPoint = CurrentPath.GetClosestWaypoint(position);
            
            if (CurrentPath.TryGetNextWaypoint(_currentWayPoint, out var nextWayPoint))
            {
                _nextWayPoint = nextWayPoint;
            }
        }
        
        public void SetRouteToDestination(RouteData data, Waypoint destination)
        {
            _currentRoute = data;
            _currentPathIndex = 0;
            _destinationWayPoint = destination;
            Car.SetState(new MovingState());
        }

        private void DetectPoints()
        {
            if (Vector2.Distance(Car.Position, _nextWayPoint.transform.position) < WaypointDetectionDistance)
            {
                GetNextWayPoint();
            }
        }

        private void GetNextWayPoint()
        {
            _currentWayPoint = _nextWayPoint;

            if (_currentWayPoint == _destinationWayPoint)
            {
                ReachedDestination();
                return;
            }
            if (CurrentPath.TryGetNextWaypoint(_currentWayPoint, out var nextWayPoint))
            {
                _nextWayPoint = nextWayPoint;
            }
            else
            {
                ChangePath();
            }
        }

        private void ReachedDestination()
        {
            _destinationWayPoint = null;
            Car.SetState(new IdleState());
        }

        private void ChangePath()
        {
            _currentPathIndex++;
            CurrentPath = _currentRoute.Paths[_currentPathIndex];
            _nextWayPoint = CurrentPath.StartPoint;
        }
        
        public Vector2 GetNextPosition => _nextWayPoint? _nextWayPoint.transform.position : Vector2.zero;
        public bool IsNextPositionDestination => _nextWayPoint == _destinationWayPoint;
        public bool IsNextWaypointLast => _nextWayPoint == CurrentPath.EndPoint;
        
    }
}