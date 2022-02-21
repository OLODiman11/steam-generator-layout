using System.Collections.Generic;

namespace SteamGeneratorLayout
{
    public class Package
    {
        public readonly float X;
        public readonly float Y;

        private List<Tube> _tubes;
        public IEnumerable<Tube> Tubes => _tubes;

        public Package(float x, float y)
        {
            X = x;
            Y = y;

            _tubes = new List<Tube>(4000);
            LayoutTubes();
        }

        private void LayoutTubes()
        {
            var y = Y + GeometryData.TubeDiameter / 2;

            while(y  < GeometryData.InnerDiameter)
            {
                var x = X + GeometryData.TubeDiameter / 2;
                for (int i = 0; i < GeometryData.PackageWidth; i++)
                {
                    if(DoesFit(x, y)) _tubes.Add(new Tube(x, y));

                    x += GeometryData.HorizontalStep;
                }
                y += GeometryData.VerticalStep;
            }
        }

        private bool DoesFit(float x, float y)
        {
            x -= GeometryData.InnerDiameter / 2;
            y -= GeometryData.InnerDiameter / 2;
            var sqrMagnitude = x * x + y * y;
            var centerRadius = GeometryData.PackageDiameter / 2 - GeometryData.TubeDiameter / 2;
            return sqrMagnitude <= centerRadius * centerRadius;
        }
    }
}
