using UnityEngine;

namespace Model.Car.Components
{
    public class CarRotation: CarComponent
    {
        [SerializeField] private float _rotationMultiplier = 30;

        public void UpdateRotation(Vector2 destination, float currentSpeed)
        {
            RotateTowardsPoint(destination, currentSpeed);
        }

        public float GetRotationAmount(Vector2 pos, Vector2 destination)
        {
            var rotation = Car.transform.rotation;
            
            var currentAngle = rotation.eulerAngles.z;

            return Mathf.Abs(Mathf.DeltaAngle(currentAngle, GetAngle(pos, destination)));
        }

        private void RotateTowardsPoint(Vector2 destination, float currentSpeed)
        {
            var angle = GetAngle(Car.Position, destination);
            
            var targetRotation = Quaternion.Euler(0, 0, angle);
            Car.transform.rotation = Quaternion.Slerp(Car.transform.rotation, targetRotation,
                currentSpeed / _rotationMultiplier * Time.deltaTime);
        }

        private float GetAngle(Vector2 position, Vector2 destination)
        {
            var direction = (destination - position).normalized;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }
}