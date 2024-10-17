﻿using Model.Paths;
using Zenject;

namespace Bootstrap
{
    public class GameplayInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PathsManager>().FromInstance((PathsManager)FindObjectOfType(typeof(PathsManager))).AsSingle().NonLazy();
        }
    }
}