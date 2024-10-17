using Model.Car;
using Model.Paths;
using Zenject;

namespace Bootstrap
{
    public class GameplayInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PathsManager>().FromInstance((PathsManager)FindObjectOfType(typeof(PathsManager))).AsSingle().NonLazy();
            Container.Bind<Car>().FromInstance((Car)FindObjectOfType(typeof(Car))).AsSingle().NonLazy();

        }
    }
}