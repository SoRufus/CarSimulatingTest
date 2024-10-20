using Model.Car;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace View.Debug
{
    public class DebugOptions : MonoBehaviour
    {
        [Inject] private readonly Car _car;

        [SerializeField] private Slider _maxSpeedSlider;
        [SerializeField] private Slider _accelerationSlider;
        [SerializeField] private Slider _deceleration;
        [SerializeField] private Slider _decelerationOnTurns;

        [SerializeField] private TextMeshProUGUI _maxSpeedValueText;
        [SerializeField] private TextMeshProUGUI _accelerationValueText;
        [SerializeField] private TextMeshProUGUI _decelerationValueText;
        [SerializeField] private TextMeshProUGUI _decelerationOnTurnsValueText;

        private void OnEnable()
        {
            _maxSpeedSlider.SetValueWithoutNotify(_car.StatsConfig.MaxSpeed.Value);
            _accelerationSlider.SetValueWithoutNotify(_car.StatsConfig.Acceleration.Value);
            _deceleration.SetValueWithoutNotify(_car.StatsConfig.Deceleration.Value);
            _decelerationOnTurns.SetValueWithoutNotify(_car.StatsConfig.DecelerationOnCrossRoads.Value);
            
            _maxSpeedValueText.text = _car.StatsConfig.MaxSpeed.Value.ToString("f2");
            _accelerationValueText.text = _car.StatsConfig.Acceleration.Value.ToString("f2");
            _decelerationValueText.text = _car.StatsConfig.Deceleration.Value.ToString("f2");
            _decelerationOnTurnsValueText.text = _car.StatsConfig.DecelerationOnCrossRoads.Value.ToString("f2");

            
            _maxSpeedSlider.onValueChanged.AddListener(OnMaxSpeedChanged);
            _accelerationSlider.onValueChanged.AddListener(OnAccelerationChanged);
            _deceleration.onValueChanged.AddListener(OnDecelerationChanged);
            _decelerationOnTurns.onValueChanged.AddListener(OnDecelerationOnTurnsChanged);
        }

        private void OnDisable()
        {
            _maxSpeedSlider.onValueChanged.RemoveListener(OnMaxSpeedChanged);
            _accelerationSlider.onValueChanged.RemoveListener(OnAccelerationChanged);
            _deceleration.onValueChanged.RemoveListener(OnDecelerationChanged);
            _decelerationOnTurns.onValueChanged.RemoveListener(OnDecelerationOnTurnsChanged);
        }

        private void OnMaxSpeedChanged(float value)
        {
            _car.StatsConfig.MaxSpeed.SetValue(value);
            _maxSpeedValueText.text = value.ToString("f2");
        }
        
        private void OnAccelerationChanged(float value)
        {
            _car.StatsConfig.Acceleration.SetValue(value);
            _accelerationValueText.text = value.ToString("f2");
        }
        
        private void OnDecelerationChanged(float value)
        {
            _car.StatsConfig.Deceleration.SetValue(value);
            _decelerationValueText.text = value.ToString("f2");
        }
        
        private void OnDecelerationOnTurnsChanged(float value)
        {
            _car.StatsConfig.DecelerationOnCrossRoads.SetValue(value);
            _decelerationOnTurnsValueText.text = value.ToString("f2");
        }
    }
}
