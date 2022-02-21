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
            CalculateRatio();
            SetGeometryAndCreateGenerator();
        }

        private void CalculateRatio()
        {
            var panel = splitContainer1.Panel2;
            Ratio = (panel.Width - panel.Padding.Left - panel.Padding.Right) / (float)innerDiameter.Value;
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
            CalculateRatio();
            SetGeometryAndCreateGenerator();
            splitContainer1.Panel2.Invalidate();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            if (_steamGenerator == null) return;

            var g = e.Graphics;
            var panel = splitContainer1.Panel2;
            var startX = panel.Padding.Left;
            var startY = panel.Padding.Right;
            var panelWidth = panel.Width - panel.Padding.Left - panel.Padding.Right;
            var panelHeight = panel.Height - panel.Padding.Top - panel.Padding.Bottom;

            var generatorDiameter = (float)innerDiameter.Value;
            g.DrawEllipse(Pens.Blue, startX, startY, generatorDiameter * Ratio, generatorDiameter * Ratio);

            var packages = _steamGenerator.Packages;
            var diameter = (float)tubeDiameter.Value;
            foreach (var package in packages)
            {
                foreach (var tube in package.Tubes)
                {
                    var lrCornerX = tube.X - diameter / 2;
                    var lrCornerY = tube.Y - diameter / 2;
                    g.DrawEllipse(Pens.Blue, startX + lrCornerX * Ratio, startY + lrCornerY * Ratio, diameter * Ratio, diameter * Ratio);
                }
            }
        }
    }
}
