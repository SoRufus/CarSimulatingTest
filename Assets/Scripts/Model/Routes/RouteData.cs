using System.Collections.Generic;
using Model.Paths;


namespace Model.Routes
{
    public struct RouteData
    {
        public List<Path> Paths { get; private set; }
        public float Distance { get; private set; }

        public RouteData(List<Path> paths, float distance)
        {
            Paths = paths;
            Distance = distance;
        }
    }
}