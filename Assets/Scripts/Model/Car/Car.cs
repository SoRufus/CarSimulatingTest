using UnityEngine;

namespace Model.Car
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _acceleration = 1f;
        [SerializeField] private float _deceleration = 1f;
        [SerializeField] private float _maxSpeed = 10f;

        private float _currentSpeed;
    }
}
