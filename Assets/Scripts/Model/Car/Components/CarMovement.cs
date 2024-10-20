using R3;
using UnityEngine;

namespace Model.Car.Components
{
    public class CarMovement: CarComponent
    {
        private readonly ReactiveProperty<float> _currentSpeed = new();
        
        private float _currentAcceleration;
        private Vector2 _previousPosition;
        
        private const float SpeedThreshold = 2f;

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
        
        private void RecalculateAcceleration(bool isNextPositionDestination, bool isNextWaypointLast, float rotationAmount)
        {
            if (isNextPositionDestination)
            {
                Decelerate(StatsConfig.Deceleration.Value);
            }
            else if (isNextWaypointLast && _currentSpeed.Value > StatsConfig.MaxSpeed.Value / SpeedThreshold)
            {
                Decelerate(StatsConfig.DecelerationOnCrossRoads.Value);
            }
            else
            {
                Accelerate(rotationAmount);
            }
        }

        private void Accelerate(float rotationAmount)
        {
            if (_currentSpeed.Value > StatsConfig.MaxSpeed.Value / SpeedThreshold)
            {
                _currentAcceleration = Mathf.Min(StatsConfig.MaxSpeed.Value,
                    _currentAcceleration + (StatsConfig.Acceleration.Value * Time.deltaTime) / Mathf.Max(1, rotationAmount));
            }
            else
            {
                _currentAcceleration = Mathf.Min(StatsConfig.MaxSpeed.Value,
                    _currentAcceleration + StatsConfig.Acceleration.Value * Time.deltaTime);
            }
        }
        
        private void Decelerate(float decelerationRate)
        {
            _currentAcceleration = Mathf.Max(StatsConfig.MinSpeed.Value,
                _currentAcceleration - decelerationRate * _currentAcceleration * Time.deltaTime);
        }
        
        private void UpdateSpeed()
        {
            var currentPosition = Car.Position;
            var distanceTraveled = Vector2.Distance(currentPosition, _previousPosition);
            _currentSpeed.Value = distanceTraveled / Time.fixedDeltaTime;
            _currentSpeed.Value = _currentSpeed.Value < 0.1f ? 0 : _currentSpeed.Value;
            _previousPosition = currentPosition;
        }
        
        public ReadOnlyReactiveProperty<float> CurrentSpeed => _currentSpeed.ToReadOnlyReactiveProperty();
    }
}