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
            var packageWidth = GeometryData.HorizontalStep * (GeometryData.PackageWidth - 1);
            var sum = 0f;
            for(int i = 0; i < 4; i++)
            {
                sum += GeometryData.PassageWidths[i];
                var position = new Vector2(GeometryData.SideDistance + packageWidth * i + sum, y);
                _packages.Add(new Package(position));
            }
        }
    }
}
