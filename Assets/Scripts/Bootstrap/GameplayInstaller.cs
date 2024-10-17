using Model.Paths;
using Zenject;

namespace Bootstrap
{
    public class GameplayInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            //Container.Bind<PathsManager>().AsSingle();
        }
    }
}