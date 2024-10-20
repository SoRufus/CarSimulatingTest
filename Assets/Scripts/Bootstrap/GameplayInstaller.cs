using Model.Car;
using Zenject;

namespace Bootstrap
{
    public class GameplayInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Car>().FromInstance((Car)FindObjectOfType(typeof(Car))).AsSingle().NonLazy();
        }
    }
}