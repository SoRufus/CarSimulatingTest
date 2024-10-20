using R3;
using UnityEngine;

namespace Model.Car.Components
{
    public class CarMovement: CarComponent
    {
        private readonly ReactiveProperty<float> _currentSpeed = new();
        
        private float _currentAcceleration;
        private Vector2 _previousPosition;

        public void UpdateMovement(Vector2 nextPosition, bool isNextPositionDestination = false,
            bool isNextWaypointLast = false, float rotationAmount = 0)
        {
            RecalculateAcceleration(isNextPositionDestination, isNextWaypointLast, rotationAmount);
            MoveTowardPosition(nextPosition);
            UpdateSpeed();
        }

        public void SetAcceleration(float value)
        {
            _currentAcceleration = value;
        }

        private void MoveTowardPosition(Vector2 nextPosition)
        {
            Car.transform.position = Vector2.MoveTowards(Car.Position, nextPosition,
                _currentAcceleration * Time.deltaTime);
        }

        private void RecalculateAcceleration(bool isNextPositionDestination, bool isNextWayPointLast, float rotationAmount)
        {
            if (isNextPositionDestination)
            {
                _currentAcceleration = Mathf.Max(StatsConfig.MinSpeed.Value, 
                    _currentAcceleration - StatsConfig.Deceleration.Value * _currentAcceleration * Time.deltaTime);
            }
            else if (isNextWayPointLast)
            {
                _currentAcceleration = Mathf.Max(StatsConfig.MinSpeed.Value, 
                    _currentAcceleration - StatsConfig.DecelerationOnCrossRoads.Value * _currentAcceleration * Time.deltaTime);
            }
            else
            {
                _currentAcceleration = Mathf.Min(StatsConfig.MaxSpeed.Value, 
                    _currentAcceleration + StatsConfig.Acceleration.Value * Time.deltaTime);
            }
        }
        
        private void UpdateSpeed()
        {
            var currentPosition = Car.Position;
            var distanceTraveled = Vector2.Distance(currentPosition, _previousPosition);
            _currentSpeed.Value = distanceTraveled / Time.deltaTime;
            _previousPosition = currentPosition;
        }
        
        public ReadOnlyReactiveProperty<float> CurrentSpeed => _currentSpeed.ToReadOnlyReactiveProperty();
    }
}