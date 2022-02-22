using System.Collections.Generic;
using System.Numerics;

namespace SteamGeneratorLayout
{
    public class Package
    {
        public readonly Vector2 Position;
        public readonly List<Vector2> Tubes;

        public Package(Vector2 position)
        {
            Position = position;

            Tubes = new List<Vector2>(3500);
            LayoutTubes();
        }

        private void LayoutTubes()
        {
            Vector2 position = Vector2.Zero;
            int j = 0;
            while(position.Y  < GeometryData.InnerDiameter)
            {
                for (int i = 0; i < GeometryData.PackageWidth; i++)
                {
                    var offset = new Vector2(GeometryData.TubeDiameter / 2);
                    var step = new Vector2(GeometryData.HorizontalStep * i, GeometryData.VerticalStep * j);
                    position = Position + offset + step;
                    if(DoesFit(position)) Tubes.Add(position);
                }
                j++;
            }
        }

        private bool DoesFit(Vector2 position)
        {
            var vector = position - new Vector2(GeometryData.InnerDiameter / 2);
            var radiusToTubeCenter = (GeometryData.PackageDiameter - GeometryData.TubeDiameter) / 2;
            return vector.LengthSquared() <= radiusToTubeCenter * radiusToTubeCenter;
        }
    }
}
