using Model.Car;
using Model.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace View.Debug
{
    public class DebugSlider: MonoBehaviour
    {
        [Inject] private readonly Car _car;
        
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _valueText;

        private StatData _data;
        
        public void Init(StatData data)
        {
            _data = data;
            
            _valueText.text = data.Value.ToString("f2");
            _slider.SetValueWithoutNotify(data.Value);
        }
        
        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(OnValueChanged);
        }
        
        private void OnValueChanged(float value)
        {
            _data.SetValue(value);
            _valueText.text = value.ToString("f2");
        }
    }
}