using System;
using Model.Car;
using UnityEngine.Device;
using Zenject;

namespace Bootstrap
{
    public class GameplayInstaller: MonoInstaller
    {
        //debug
        private void OnEnable()
        {
            Application.targetFrameRate = 60;
        }
        
        public override void InstallBindings()
        {
            Container.Bind<Car>().FromInstance((Car)FindObjectOfType(typeof(Car))).AsSingle().NonLazy();
        }
    }
}