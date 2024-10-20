using Model.Car;
using UnityEngine;
using Zenject;

namespace View.Debug
{
    public class DebugOptions : MonoBehaviour
    {
        [Inject] private readonly Car _car;

        [SerializeField] private DebugSlider _maxSpeedSlider;
        [SerializeField] private DebugSlider _accelerationSlider;
        [SerializeField] private DebugSlider _deceleration;
        [SerializeField] private DebugSlider _decelerationOnCrossRoads;

        private void OnEnable()
        {
            _maxSpeedSlider.Init(_car.StatsConfig.MaxSpeed);
            _accelerationSlider.Init(_car.StatsConfig.Acceleration);
            _deceleration.Init(_car.StatsConfig.Deceleration);
            _decelerationOnCrossRoads.Init(_car.StatsConfig.DecelerationOnCrossRoads);
        }
    }
}
