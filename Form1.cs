using System;
using System.Drawing;
using System.Windows.Forms;

namespace SteamGeneratorLayout
{
    public partial class Form1 : Form
    {
        public static float Ratio { get; private set; }

        private SteamGenerator _steamGenerator;

        public Form1()
        {
            InitializeComponent();
            SetGeometryAndCreateGenerator();
            var panel = splitContainer1.Panel2;
            Ratio = (panel.Width - panel.Padding.Left - panel.Padding.Right) / (float) innerDiameter.Value;
            Paint += DrawGenerator;
        }

        private void SetGeometryAndCreateGenerator()
        {
            GeometryData.SetGeometry(
                (float)innerDiameter.Value,
                (float)horizontalStep.Value,
                (float)verticalStep.Value,
                (float)packageDiameter.Value,
                (float)sideDistance.Value,
                (float)tubeDiameter.Value,
                (float)distanceFromHorizontalAxis.Value,
                (int)packageWidth.Value);

            _steamGenerator = new SteamGenerator();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            SetGeometryAndCreateGenerator();
        }

        private void DrawGenerator(object sender, PaintEventArgs e)
        {
            if (_steamGenerator == null) return;

            var g = e.Graphics;
            var packages = _steamGenerator.Packages;
            var diameter = (float) tubeDiameter.Value;
            foreach (var package in packages)
            {
                foreach (var tube in package.Tubes)
                {
                    var lrCornerX = tube.X - diameter;
                    var lrCornerY = tube.Y - diameter;
                    g.DrawEllipse(Pens.Blue, lrCornerX * Ratio, lrCornerY * Ratio, diameter, diameter);
                }
            }
        }
    }
}
