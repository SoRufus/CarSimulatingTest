using Model.Stats;
using UnityEngine;

namespace Model.Car.Components
{
    [RequireComponent(typeof(Car))]
    public abstract class CarComponent: MonoBehaviour
    {
        protected Car Car;
        protected StatsConfig StatsConfig;
        
        protected virtual void OnEnable()
        {
            Car = GetComponent<Car>();
            StatsConfig = Car.StatsConfig;
        }
    }
}