using System.Collections.Generic;
using System.Numerics;

namespace SteamGeneratorLayout
{
    public class SteamGenerator
    {
        private List<Package> _packages;
        public List<Package> Packages => _packages;

        public SteamGenerator()
        {
            _packages = new List<Package>(4);

            CreatePackages();
        }

        private void CreatePackages()
        {
            var y = GeometryData.InnerDiameter / 2 - GeometryData.DistanceFromHorizontalAxis;
            var packageWidth = GeometryData.HorizontalStep * (GeometryData.PackageWidth - 1) + GeometryData.TubeDiameter;
            var gap = (GeometryData.InnerDiameter - 2 * GeometryData.SideDistance - 4 * packageWidth) / 3;

            for(int i = 0; i < 4; i++)
            {
                var position = new Vector2(GeometryData.SideDistance + (packageWidth + gap) * i, y);
                _packages.Add(new Package(position));
            }
        }
    }
}
