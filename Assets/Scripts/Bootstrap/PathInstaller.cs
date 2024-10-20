using System.Collections.Generic;
using System.Linq;
using Model.Paths;
using Model.Routes;
using UnityEngine;
using Zenject;

namespace Bootstrap
{
    public class PathInstaller: MonoInstaller
    {
        [SerializeField] private List<Path> _paths = new();

        [ContextMenu(nameof(GetPathsToList))]
        public void GetPathsToList()
        {
            _paths = FindObjectsOfType<Path>().ToList();
        }

        public override void InstallBindings()
        {
            Container.Bind<PathFinder>().AsSingle().WithArguments(_paths).NonLazy();
            Container.Bind<RouteFinder>().AsSingle().NonLazy();
        }
    }
}