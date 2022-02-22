using System;
using System.Collections.Generic;
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
            CalculateAndSetPassageWidth();
            DontCalculateWidths();
        }

        private void DontCalculateWidths()
        {
            GeometryData.SetGeometry(
                            (float)innerDiameter.Value,
                            (float)horizontalStep.Value,
                            (float)verticalStep.Value,
                            (float)(innerDiameter.Value - 2 * packageDiameter.Value),
                            (float)sideDistance.Value,
                            (float)tubeDiameter.Value,
                            (float)distanceFromHorizontalAxis.Value,
                            (int)packageWidth.Value,
                            new List<float>()
                            {
                    0f, (float)leftPassageWidth.Value, (float)centerPassageWidth.Value, float.Parse(rightPassageWidth.Text)
                            });

            _steamGenerator = new SteamGenerator();
            SetLabels();
            CalculateAndSetFs();
            pictureBox1.Invalidate();
        }

        private void CalculateAndSetFs()
        {
            var fPassageCenter = centerPassageWidth.Value / 2 + (leftPassageWidth.Value + decimal.Parse(rightPassageWidth.Text)) / 4 - tubeDiameter.Value;
            var fPassageSide = sideDistance.Value + (leftPassageWidth.Value + decimal.Parse(rightPassageWidth.Text)) / 4 - tubeDiameter.Value;
            var fPackageCenter = (horizontalStep.Value - tubeDiameter.Value) * (packageWidth.Value - 1);
            var fPackageSide = fPackageCenter;
            var fBelowCenter = packageDiameter.Value - tubeDiameter.Value / 2;
            var fBelowSide = fBelowCenter;
            var fCommonCenter = decimal.Parse(packageWidthMM.Text) + tubeDiameter.Value;
            var fCommonSide = fCommonCenter;

            this.fPackageCenter.Text = Math.Round(fPackageCenter, 0).ToString();
            this.fPackageSide.Text = Math.Round(fPackageSide, 0).ToString();
            this.fPassageCenter.Text = Math.Round(fPassageCenter, 0).ToString();
            this.fPassageSide.Text = Math.Round(fPassageSide, 0).ToString();
            this.fBelowCenter.Text = Math.Round(fBelowCenter, 0).ToString();
            this.fBelowSide.Text = Math.Round(fBelowSide, 0).ToString();
            this.fCommonCenter.Text = Math.Round(fCommonCenter, 0).ToString();
            this.fCommonSide.Text = Math.Round(fCommonSide, 0).ToString();

            fPackPassCenter.Text = Math.Round(fPackageCenter / fPassageCenter, 3).ToString();
            fPackPassSide.Text = Math.Round(fPackageSide / fPassageSide, 3).ToString();
            fPackBelCenter.Text = Math.Round(fPackageCenter / fBelowCenter, 3).ToString();
            fPackBelSide.Text = Math.Round(fPackageSide / fBelowSide, 3).ToString();
            fComPackCenter.Text = Math.Round(fCommonCenter / fPackageCenter, 3).ToString();
            fComPackSide.Text = Math.Round(fCommonSide / fPackageSide, 3).ToString();
        }

        private void SetLabels()
        {
            var counts = new int[4];
            int i = 0;
            foreach (var package in _steamGenerator.Packages) counts[i++] = package.Tubes.Count;
            tubesCount1.Text = counts[0].ToString();
            tubesCount2.Text = counts[1].ToString();
            tubesCount3.Text = counts[2].ToString();
            tubesCount4.Text = counts[3].ToString();
            tubesCount.Text = (counts[0] + counts[1] + counts[2] + counts[3]).ToString();
            packageWidthMM.Text = (horizontalStep.Value * (packageWidth.Value - 1)).ToString();
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

        private void innerDiameter_ValueChanged(object sender, EventArgs e) => SetGeometryAndCreateGenerator();

        private void sideDistance_ValueChanged(object sender, EventArgs e) => SetGeometryAndCreateGenerator();

        private void packageWidth_ValueChanged(object sender, EventArgs e) => SetGeometryAndCreateGenerator();

        private void horizontalStep_ValueChanged(object sender, EventArgs e) => SetGeometryAndCreateGenerator();

        private void CalculateAndSetPassageWidth()
        {
            var summ = innerDiameter.Value - 2 * sideDistance.Value - 4 * horizontalStep.Value * (packageWidth.Value - 1);
            if (summ < 0) return;
            summPassageWidth.Text = (summ).ToString();
            centerPassageWidth.Maximum = summ;
            leftPassageWidth.Maximum = summ;
            rightPassageWidth.Text = (summ / 3).ToString();
            centerPassageWidth.Value = summ / 3;
            leftPassageWidth.Value = summ / 3;
        }

        private void leftPassageWidth_ValueChanged(object sender, EventArgs e)
        {
            AdjustPassageWidths(leftPassageWidth, centerPassageWidth);
            DontCalculateWidths();
        }

        private void centerPassageWidth_ValueChanged(object sender, EventArgs e)
        {
            AdjustPassageWidths(centerPassageWidth, leftPassageWidth);
            DontCalculateWidths();
        }

        private void AdjustPassageWidths(NumericUpDown set, NumericUpDown second)
        {
            var first = decimal.Parse(rightPassageWidth.Text);
            var offset = decimal.Parse(summPassageWidth.Text) - set.Value - first - second.Value;
            if(-offset > first)
            {
                second.Value += first + offset;
                first = 0;
            }
            else
            {
                first += offset;
            }
            rightPassageWidth.Text = Math.Round(first, 1).ToString();
        }

        private void verticalStep_ValueChanged(object sender, EventArgs e) => SetGeometryAndCreateGenerator();

        private void tubeDiameter_ValueChanged(object sender, EventArgs e) => SetGeometryAndCreateGenerator();

        private void packageDiameter_ValueChanged(object sender, EventArgs e) => SetGeometryAndCreateGenerator();

        private void distanceFromHorizontalAxis_ValueChanged(object sender, EventArgs e) => SetGeometryAndCreateGenerator();
    }
}
