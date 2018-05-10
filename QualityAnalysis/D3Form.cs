using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace QualityDim
{
    public partial class D3Form : Form
    {
        private Device device = null;
        double[,] matrMin, matrMax;
        bool[] arrTexture;
        public D3Form(double[,] matrMin, double[,] matrMax, bool[] arrTexture)
        {
            double xMin, yMin, zMin, xMax, yMax, zMax;
            xMin = yMin = zMin = double.MaxValue;
            xMax = yMax = zMax = double.MinValue;
            for (int i = 0; i < matrMin.GetLength(0); i++)
            {
                if (matrMin[i, 0] < xMin)
                    xMin = matrMin[i, 0];
                if (matrMin[i, 1] < yMin)
                    yMin = matrMin[i, 1];
                if (matrMin[i, 2] < zMin)
                    zMin = matrMin[i, 2];
                if (matrMax[i, 0] > xMax)
                    xMax = matrMax[i, 0];
                if (matrMax[i, 1] > yMax)
                    yMax = matrMax[i, 1];
                if (matrMax[i, 2] > zMax)
                    zMax = matrMax[i, 2];
            }
            this.matrMin = matrMin;
            this.matrMax = matrMax;
            this.arrTexture = arrTexture;
            for (int i = 0; i < matrMin.GetLength(0); i++)
            {
                matrMin[i, 0] = (matrMin[i, 0] - xMin) / (xMax - xMin);
                matrMin[i, 1] = (matrMin[i, 1] - yMin) / (yMax - yMin);
                matrMin[i, 2] = (matrMin[i, 2] - zMin) / (zMax - zMin);
                matrMax[i, 0] = (matrMax[i, 0] - xMin) / (xMax - xMin);
                matrMax[i, 1] = (matrMax[i, 1] - yMin) / (yMax - yMin);
                matrMax[i, 2] = (matrMax[i, 2] - zMin) / (zMax - zMin);
            }
            InitializeComponent();
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
            try
            {
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.White, 1.0f, 0);
                SetupCamera();
                device.BeginScene();
                for (int i = 0; i < matrMin.GetLength(0); i++)
                    DrawCube((float)matrMin[i, 0], (float)matrMax[i, 0], (float)matrMin[i, 1],
                        (float)matrMax[i, 1], (float)matrMin[i, 2], (float)matrMax[i, 2], arrTexture[i]);
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
                device.EndScene();
                device.Present();
            }
            catch { }
        }
        void SetupCamera()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 20, 1, 1, 100);
            device.Transform.View = Matrix.LookAtLH(new Vector3(8, 17, 8), new Vector3(0, 0, 1), new Vector3(0, 0, 1));
            device.RenderState.CullMode = Cull.None;
        }
        void DrawCube(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax, bool texture)
        {
            float tmp;
            tmp = xMin; xMin = yMin; yMin = tmp;
            tmp = xMax; xMax = yMax; yMax = tmp;
            List<CustomVertex.PositionTextured> listV = new List<CustomVertex.PositionTextured>();
            CustomVertex.PositionTextured[] arrV = new CustomVertex.PositionTextured[]
            {
                        new CustomVertex.PositionTextured(xMin, yMin, zMin, xMin, zMin),
                        new CustomVertex.PositionTextured(xMax, yMin, zMin, xMax, zMin),
                        new CustomVertex.PositionTextured(xMax, yMin, zMax, xMax, zMax),
                        new CustomVertex.PositionTextured(xMin, yMin, zMin, xMin, zMin),
                        new CustomVertex.PositionTextured(xMax, yMin, zMax, xMax, yMax),
                        new CustomVertex.PositionTextured(xMin, yMin, zMax, xMin, zMax),

                        new CustomVertex.PositionTextured(xMax, yMin, zMin, yMin, zMin),
                        new CustomVertex.PositionTextured(xMax, yMax, zMin, yMax, zMin),
                        new CustomVertex.PositionTextured(xMax, yMax, zMax, yMax, zMax),
                        new CustomVertex.PositionTextured(xMax, yMin, zMin, yMin, zMin),
                        new CustomVertex.PositionTextured(xMax, yMax, zMax, yMax, zMax),
                        new CustomVertex.PositionTextured(xMax, yMin, zMax, yMin, zMax),

                        new CustomVertex.PositionTextured(xMax, yMax, zMin, xMax, zMin),
                        new CustomVertex.PositionTextured(xMin, yMax, zMin, xMin, zMin),
                        new CustomVertex.PositionTextured(xMin, yMax, zMax, xMin, zMax),
                        new CustomVertex.PositionTextured(xMax, yMax, zMin, xMax, zMin),
                        new CustomVertex.PositionTextured(xMin, yMax, zMax, xMin, zMax),
                        new CustomVertex.PositionTextured(xMax, yMax, zMax, xMax, zMax),

                        new CustomVertex.PositionTextured(xMin, yMax, zMin, yMax, zMin),
                        new CustomVertex.PositionTextured(xMin, yMin, zMin, yMin, zMin),
                        new CustomVertex.PositionTextured(xMin, yMin, zMax, yMin, zMax),
                        new CustomVertex.PositionTextured(xMin, yMax, zMin, yMax, zMin),
                        new CustomVertex.PositionTextured(xMin, yMin, zMax, yMin, zMax),
                        new CustomVertex.PositionTextured(xMin, yMax, zMax, yMax, zMax),

                        new CustomVertex.PositionTextured(xMin, yMin, zMax, xMin, yMin),
                        new CustomVertex.PositionTextured(xMax, yMin, zMax, xMax, yMin),
                        new CustomVertex.PositionTextured(xMax, yMax, zMax, xMax, yMax),
                        new CustomVertex.PositionTextured(xMin, yMin, zMax, xMin, yMin),
                        new CustomVertex.PositionTextured(xMax, yMax, zMax, xMax, yMax),
                        new CustomVertex.PositionTextured(xMin, yMax, zMax, xMin, yMax),

                        new CustomVertex.PositionTextured(xMin, yMin, zMin, xMin, yMin),
                        new CustomVertex.PositionTextured(xMax, yMin, zMin, xMax, yMin),
                        new CustomVertex.PositionTextured(xMax, yMax, zMin, xMax, yMax),
                        new CustomVertex.PositionTextured(xMin, yMin, zMin, xMin, yMin),
                        new CustomVertex.PositionTextured(xMax, yMax, zMin, xMax, yMax),
                        new CustomVertex.PositionTextured(xMin, yMax, zMin, xMin, yMax)
            };
            int lc1 = Color.Black.ToArgb();
            CustomVertex.PositionColored[] arrVL = new CustomVertex.PositionColored[]
            {
                        new CustomVertex.PositionColored(xMin, yMin, zMin, lc1),
                        new CustomVertex.PositionColored(xMax, yMin, zMin, lc1),
                        new CustomVertex.PositionColored(xMax, yMax, zMin, lc1),
                        new CustomVertex.PositionColored(xMin, yMax, zMin, lc1),
                        new CustomVertex.PositionColored(xMin, yMin, zMax, lc1),
                        new CustomVertex.PositionColored(xMax, yMin, zMax, lc1),
                        new CustomVertex.PositionColored(xMax, yMax, zMax, lc1),
                        new CustomVertex.PositionColored(xMin, yMax, zMax, lc1)
            };
            int[] arrIL = { 0, 1, 1, 2, 2, 3, 3, 0, 0, 4, 1, 5, 2, 6, 3, 7, 4, 5, 5, 6, 6, 7, 7, 4 };
            CustomVertex.PositionColored[] arrVL2 = new CustomVertex.PositionColored[arrIL.Length];
            for (int k = 0; k < arrIL.Length; k++)
                arrVL2[k] = arrVL[arrIL[k]];
            device.RenderState.Lighting = false;
            if (texture)
            {
                device.VertexFormat = CustomVertex.PositionTextured.Format;
                Texture t = new Texture(device, Properties.Resources.Tex1, 0, Pool.Managed);
                device.SetTexture(0, t);
                device.DrawUserPrimitives(PrimitiveType.TriangleList, arrV.Length / 3, arrV);
            }
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.DrawUserPrimitives(PrimitiveType.LineList, arrVL2.Length / 2, arrVL2);
        }
    }
}
