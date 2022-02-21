using System.Collections.Generic;

namespace SteamGeneratorLayout
{
    public class SteamGenerator
    {
        private List<Package> _packages;
        public IEnumerable<Package> Packages => _packages;

        public SteamGenerator()
        {
            _packages = new List<Package>(4);

            CreatePackages();
        }

        private void CreatePackages()
        {
            var x = GeometryData.SideDistance;
            var y = GeometryData.InnerDiameter / 2 - GeometryData.DistanceFromHorizontalAxis;
            var packageWidth = GeometryData.HorizontalStep * GeometryData.PackageWidth;
            var gap = (GeometryData.InnerDiameter - 2 * x - 4 * packageWidth) / 4;

            for(int i = 0; i < 4; i++)
            {
                _packages.Add(new Package(x, y));
                x += packageWidth + gap;
            }
        }
    }
}
