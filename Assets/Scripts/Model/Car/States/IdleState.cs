namespace Model.Car.States
{
    public class IdleState : CarState
    {
        public override void OnStateApplied(Car car)
        {
            car.Movement.SetAcceleration(0);
        }
    }
}