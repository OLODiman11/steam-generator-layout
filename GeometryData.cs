namespace SteamGeneratorLayout
{
    public static class GeometryData
    {
        public static float InnerDiameter { get; private set; }
        public static float HorizontalStep { get; private set; }
        public static float VerticalStep { get; private set; }
        public static float PackageDiameter { get; private set; }
        public static float SideDistance { get; private set; }
        public static float TubeDiameter { get; private set; }
        public static float DistanceFromHorizontalAxis { get; private set; }
        public static int PackageWidth { get; private set; }

        public static void SetGeometry(float innerDiameter, float horizontalStep, float verticalStep, float packageDiameter, float sideDistance, float tubeDiameter, float distanceUpFromCenter, int packageWidth)
        {
            InnerDiameter = innerDiameter;
            HorizontalStep = horizontalStep;
            VerticalStep = verticalStep;
            PackageDiameter = packageDiameter;
            SideDistance = sideDistance;
            TubeDiameter = tubeDiameter;
            DistanceFromHorizontalAxis = distanceUpFromCenter;
            PackageWidth = packageWidth;
        }
    }
}
