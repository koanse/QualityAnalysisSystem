using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace QualityDim
{
    public partial class H3DForm : Form
    {
        private Device device = null;
        double[] arrLX, arrLY, arrLZ;
        double[,] matrXY, matrXZ, matrYZ;
        bool[,] matrTXY, matrTXZ, matrTYZ;
        double zMax;
        bool legend = true;
        public H3DForm(double[] arrLX, double[] arrLY, double[] arrLZ, double[,] matrXY,
            double[,] matrXZ, double[,] matrYZ, bool[,] matrTXY, bool[,] matrTXZ, bool[,] matrTYZ, double zMax)
        {
            InitializeComponent();
            this.arrLX = arrLX;
            this.arrLY = arrLY;
            this.arrLZ = arrLZ;
            this.matrXY = matrXY;
            this.matrXZ = matrXZ;
            this.matrYZ = matrYZ;
            this.matrTXY = matrTXY;
            this.matrTXZ = matrTXZ;
            this.matrTYZ = matrTYZ;
            this.zMax = zMax;
        }
        public void InitializeGraphics()
        {
            PresentParameters presentParams = new PresentParameters();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;
            presentParams.EnableAutoDepthStencil = true;
            presentParams.AutoDepthStencilFormat = DepthFormat.D16;
            device = new Device(0, DeviceType.Hardware, this,
                CreateFlags.SoftwareVertexProcessing, presentParams);
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (legend)
            {
                LegendForm lf = new LegendForm();
                lf.Show();
                legend = false;
            }
            try
            {
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.White, 1.0f, 0);
                SetupCamera();
                device.BeginScene();
                DrawHist(arrLX, arrLY, matrXY, matrTXY, zMax, Matrix.RotationZ(0), false);
                DrawHist(arrLY, arrLZ, matrYZ, matrTYZ, zMax, Matrix.RotationY((float)Math.PI / 2) *
                    Matrix.Translation(new Vector3(0, 0, 1.0f)) * Matrix.RotationX((float)Math.PI / 2) *
                    Matrix.Translation(new Vector3(-1.4f, 1.0f, 1.0f)), false);
                DrawHist(arrLX, arrLZ, matrXZ, matrTXZ, zMax, Matrix.RotationX(-(float)Math.PI / 2) *
                    Matrix.Translation(new Vector3(0, 0, 1.0f)) * Matrix.RotationY(-(float)Math.PI / 2) *
                    Matrix.Translation(new Vector3(1.0f, -1.4f, 1.0f)), true);
                device.EndScene();
                device.Present();
            }
            catch { }
        }
        void SetupCamera()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 20, 1, 1, 100);
            device.Transform.View = Matrix.LookAtLH(new Vector3(13, 17, 15), new Vector3(0, 0, 1), new Vector3(0, 0, 1));
            device.RenderState.CullMode = Cull.None;
        }
        void DrawHist(double[] arrLX, double[] arrLY, double[,] matrZ, bool[,] matrT, double zMax, Matrix mWorld, bool swap)
        {
            if (swap)
            {
                double[] tmp = arrLX;
                arrLX = arrLY;
                arrLY = tmp;
                double[,] matrZ2 = new double[matrZ.GetLength(1), matrZ.GetLength(0)];
                bool[,] matrT2 = new bool[matrT.GetLength(1), matrT.GetLength(0)];
                for (int i = 0; i < matrZ.GetLength(0); i++)
                    for (int j = 0; j < matrZ.GetLength(1); j++)
                    {
                        matrZ2[j, i] = matrZ[i, j];
                        matrT2[j, i] = matrT[i, j];
                    }
                matrZ = matrZ2;
                matrT = matrT2;
            }
            List<CustomVertex.PositionTextured> listV = new List<CustomVertex.PositionTextured>();
            List<CustomVertex.PositionColored> listVL = new List<CustomVertex.PositionColored>();
            float xSum = 0, ySum = 0;
            for (int i = 0; i < arrLX.Length; i++)
                xSum += (float)arrLX[i];
            for (int i = 0; i < arrLY.Length; i++)
                ySum += (float)arrLY[i];
            float x0 = 0, y0 = 0;
            if (zMax == -1)
            {
                zMax = 0;
                for (int i = 0; i < matrZ.GetLength(0); i++)
                    for (int j = 0; j < matrZ.GetLength(1); j++)
                        if (matrZ[i, j] > zMax)
                            zMax = matrZ[i, j];
            }
            for (int i = 0; i < arrLX.Length; i++)
            {
                float dx = (float)arrLX[i] / xSum;
                y0 = 0;
                for (int j = 0; j < arrLY.Length; j++)
                {
                    float dy = (float)arrLY[j] / ySum, dz = (float)(matrZ[i, j] / zMax);
                    CustomVertex.PositionTextured[] arrV;
                    if (matrT[i, j])
                    {
                        CustomVertex.PositionTextured[] arrV1 = new CustomVertex.PositionTextured[]
                    {
                        new CustomVertex.PositionTextured(x0, y0, 0, 0, 0),
                        new CustomVertex.PositionTextured(x0 + dx, y0, 0, dx, 0),
                        new CustomVertex.PositionTextured(x0 + dx, y0, dz, dx, dz),
                        new CustomVertex.PositionTextured(x0, y0, 0, 0, 0),
                        new CustomVertex.PositionTextured(x0 + dx, y0, dz, dx, dy),
                        new CustomVertex.PositionTextured(x0, y0, dz, 0, dz),

                        new CustomVertex.PositionTextured(x0 + dx, y0, 0, 0, 0),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, 0, dy, 0),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, dz, dy, dz),
                        new CustomVertex.PositionTextured(x0 + dx, y0, 0, 0, 0),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, dz, dy, dz),
                        new CustomVertex.PositionTextured(x0 + dx, y0, dz, 0, dz),

                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, 0, dx, 0),
                        new CustomVertex.PositionTextured(x0, y0 + dy, 0, 0, 0),
                        new CustomVertex.PositionTextured(x0, y0 + dy, dz, 0, dz),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, 0, dx, 0),
                        new CustomVertex.PositionTextured(x0, y0 + dy, dz, 0, dz),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, dz, dx, dz),

                        new CustomVertex.PositionTextured(x0, y0 + dy, 0, dy, 0),
                        new CustomVertex.PositionTextured(x0, y0, 0, 0, 0),
                        new CustomVertex.PositionTextured(x0, y0, dz, 0, dz),
                        new CustomVertex.PositionTextured(x0, y0 + dy, 0, dy, 0),
                        new CustomVertex.PositionTextured(x0, y0, dz, 0, dz),
                        new CustomVertex.PositionTextured(x0, y0 + dy, dz, dy, dz),

                        new CustomVertex.PositionTextured(x0, y0, dz, 0, 0),
                        new CustomVertex.PositionTextured(x0 + dx, y0, dz, dx, 0),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, dz, dx, dy),
                        new CustomVertex.PositionTextured(x0, y0, dz, 0, 0),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, dz, dx, dy),
                        new CustomVertex.PositionTextured(x0, y0 + dy, dz, 0, dy)
                    };
                        arrV = arrV1;
                    }
                    else
                    {
                        CustomVertex.PositionTextured[] arrV2 = new CustomVertex.PositionTextured[]
                    {
                        new CustomVertex.PositionTextured(x0, y0, 0, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0, 0, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0, y0, 0, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0, y0, dz, 1, 1),

                        new CustomVertex.PositionTextured(x0 + dx, y0, 0, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, 0, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0, 0, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0, dz, 1, 1),

                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, 0, 1, 1),
                        new CustomVertex.PositionTextured(x0, y0 + dy, 0, 1, 1),
                        new CustomVertex.PositionTextured(x0, y0 + dy, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, 0, 1, 1),
                        new CustomVertex.PositionTextured(x0, y0 + dy, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, dz, 1, 1),

                        new CustomVertex.PositionTextured(x0, y0 + dy, 0, 1, 1),
                        new CustomVertex.PositionTextured(x0, y0, 0, 1, 1),
                        new CustomVertex.PositionTextured(x0, y0, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0, y0 + dy, 0, 1, 1),
                        new CustomVertex.PositionTextured(x0, y0, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0, y0 + dy, dz, 1, 1),

                        new CustomVertex.PositionTextured(x0, y0, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0, y0, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0 + dx, y0 + dy, dz, 1, 1),
                        new CustomVertex.PositionTextured(x0, y0 + dy, dz, 1, 1)
                    };
                        arrV = arrV2;
                    }
                    int lc1 = Color.Black.ToArgb();
                    CustomVertex.PositionColored[] arrVL = new CustomVertex.PositionColored[]
                    {
                        new CustomVertex.PositionColored(x0, y0, 0, lc1),
                        new CustomVertex.PositionColored(x0 + dx, y0, 0, lc1),
                        new CustomVertex.PositionColored(x0 + dx, y0 + dy, 0, lc1),
                        new CustomVertex.PositionColored(x0, y0 + dy, 0, lc1),
                        new CustomVertex.PositionColored(x0, y0, dz, lc1),
                        new CustomVertex.PositionColored(x0 + dx, y0, dz, lc1),
                        new CustomVertex.PositionColored(x0 + dx, y0 + dy, dz, lc1),
                        new CustomVertex.PositionColored(x0, y0 + dy, dz, lc1)
                    };
                    int[] arrIL = { 0, 1, 1, 2, 2, 3, 3, 0, 0, 4, 1, 5, 2, 6, 3, 7, 4, 5, 5, 6, 6, 7, 7, 4 };
                    for (int k = 0; k < arrIL.Length; k++)
                        listVL.Add(arrVL[arrIL[k]]);
                    listV.AddRange(arrV);
                    y0 += dy;
                }
                x0 += dx;
            }
            device.Transform.World = mWorld;
            device.VertexFormat = CustomVertex.PositionTextured.Format;
            Texture t = new Texture(device, Properties.Resources.Tex1, 0, Pool.Managed);
            device.SetTexture(0, t);
            device.RenderState.Lighting = false;
            device.DrawUserPrimitives(PrimitiveType.TriangleList, listV.Count / 3, listV.ToArray());
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.DrawUserPrimitives(PrimitiveType.LineList, listVL.Count / 2, listVL.ToArray());
            int lc2 = Color.Black.ToArgb();
            CustomVertex.PositionColored[] arrVL2 = new CustomVertex.PositionColored[]
                {
                    new CustomVertex.PositionColored(0, 0, 0, lc2),
                    new CustomVertex.PositionColored(1.2f, 0, 0, lc2),
                    new CustomVertex.PositionColored(0, 0, 0, lc2),
                    new CustomVertex.PositionColored(0, 1.2f, 0, lc2),
                    new CustomVertex.PositionColored(0, 0, 0, lc2),
                    new CustomVertex.PositionColored(0, 0, 1.2f, lc2)
                };
            device.DrawUserPrimitives(PrimitiveType.LineList, 3, arrVL2);
        }
    }
}
