using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Model.Paths
{
    public class PathsManager : MonoBehaviour
    {
        [SerializeField] private List<Path> _paths = new();

        [ContextMenu(nameof(GetPathsToList))]
        public void GetPathsToList()
        {
            _paths = GetChildComponentsToList.Get<Path>(gameObject);
        }
    }
}
