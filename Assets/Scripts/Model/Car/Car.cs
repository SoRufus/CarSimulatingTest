using Model.Car.Components;
using Model.Car.States;
using Model.Stats;
using UnityEngine;

namespace Model.Car
{
    public class Car : MonoBehaviour
    {
        [field: SerializeField] public StatsConfig StatsConfig { get; private set; }
        [field: SerializeField] public CarNavigation Navigation { get; private set; }
        [field: SerializeField] public CarRotation Rotation { get; private set; }
        [field: SerializeField] public CarMovement Movement { get; private set; }
        
        private CarState _carState = new IdleState();

        public void SetState(CarState state)
        {
            _carState = state;
            _carState.OnStateApplied(this);
        }

        private void Update()
        {
            UpdateComponents();
        }

        private void UpdateComponents()
        {
            Rotation.UpdateRotation(Navigation.GetNextPosition, Movement.CurrentSpeed.CurrentValue);
            
            if (_carState is IdleState) return;
            
            Navigation.UpdateNavigation();
            
            Movement.UpdateMovement(Navigation.GetNextPosition, Navigation.IsNextPositionDestination, 
                Navigation.IsNextWaypointLast, Rotation.GetRotationAmount(Position, Navigation.GetNextPosition));
        }
        
        public Vector2 Position => transform.position;
    }
}