using System;
using Model.Car;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace View.UI
{
    public class StatsView: MonoBehaviour
    {
        [Inject] private readonly Car _car;

        [SerializeField] private TextMeshProUGUI _speedLabel;

        private IDisposable _disposable;

        private void Awake()
        {
            var builder = Disposable.CreateBuilder();
            
            _car.Movement.CurrentSpeed.Subscribe(UpdateSpeedLabel).AddTo(ref builder);
            
            _disposable = builder.Build();
        }
        
        private void OnDisable()
        {
            _disposable?.Dispose();
        }

        private void UpdateSpeedLabel(float value)
        {
            _speedLabel.text = $"Speed: {value:F2}";
        }
    }
}