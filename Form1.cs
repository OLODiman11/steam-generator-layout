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
            var counts = new int[4];
            int i = 0;
            foreach (var package in _steamGenerator.Packages) counts[i++] = package.Tubes.Count;
            tubesCount1.Text = counts[0].ToString();
            tubesCount2.Text = counts[1].ToString();
            tubesCount3.Text = counts[2].ToString();
            tubesCount4.Text = counts[3].ToString();
            tubesCount.Text = (counts[0] + counts[1] + counts[2] + counts[3]).ToString();
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

            var generatorDiameter = innerDiameter.Value;
            g.DrawEllipse(Pens.Blue, startX, startY, (float) generatorDiameter * Ratio, (float)generatorDiameter * Ratio);

            var packages = _steamGenerator.Packages;
            var diameter = tubeDiameter.Value;
            foreach (var package in packages)
            {
                foreach (var tube in package.Tubes)
                {
                    g.DrawEllipse(Pens.Blue, startX + tube.X * Ratio, startY + tube.Y * Ratio, (float)diameter * Ratio, (float)diameter * Ratio);
                }
            }
        }
    }
}
