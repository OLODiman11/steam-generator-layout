using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Numerics;
using System.Windows.Forms;

namespace SteamGeneratorLayout
{
    public partial class Form1 : Form
    {
        private SteamGenerator _steamGenerator;

        public Form1()
        {
            InitializeComponent();
            SetGeometryAndCreateGenerator();
        }

        private float CalculateRatio(float size, float lineThickness)
        {
            return (size - lineThickness - 1) / GeometryData.InnerDiameter;
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
            pictureBox1.Invalidate();
            var counts = new int[4];
            int i = 0;
            foreach (var package in _steamGenerator.Packages) counts[i++] = package.Tubes.Count;
            tubesCount1.Text = counts[0].ToString();
            tubesCount2.Text = counts[1].ToString();
            tubesCount3.Text = counts[2].ToString();
            tubesCount4.Text = counts[3].ToString();
            tubesCount.Text = (counts[0] + counts[1] + counts[2] + counts[3]).ToString();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (_steamGenerator == null) return;

            var g = e.Graphics;
            DrawGenerator(g, pictureBox1.Width, 1);
        }

        private void DrawGenerator(Graphics g, int size, int lineThickness)
        {
            var ratio = CalculateRatio(size, lineThickness);
            var pen = new Pen(Color.Blue, lineThickness);
            var start = new Vector2(lineThickness / 2f / ratio);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var vector4 = new Vector4(start, GeometryData.InnerDiameter, GeometryData.InnerDiameter) * ratio;
            g.DrawEllipse(pen, vector4.X, vector4.Y, vector4.Z, vector4.W);


            var packages = _steamGenerator.Packages;
            var diameter = GeometryData.TubeDiameter;
            foreach (var package in packages)
            {
                foreach (var tube in package.Tubes)
                {
                    vector4 = new Vector4(start + tube, diameter, diameter) * ratio;
                    g.DrawEllipse(pen, vector4.X, vector4.Y, vector4.Z, vector4.W);
                }
            }
            pen.Dispose();
        }

        private void SaveImage(string path, int size, int lineThickness)
        {
            var ratio = (size - lineThickness - 1) / GeometryData.InnerDiameter;
            var bitMap = new Bitmap(size, size, PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(bitMap);

            DrawGenerator(g, size, lineThickness);

            bitMap.Save(path);
            g.Dispose();
            bitMap.Dispose();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveImage(@"d:/SG.png", 5000, 5);
        }
    }
}
