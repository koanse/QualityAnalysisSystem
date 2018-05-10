using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Hist3d
{
    public partial class HistForm : Form
    {
        private Device device = null;
        float[] arrLX, arrLY;
        float[,] matrZ;
        int[,] matrC;
        float xRot = 0, yRot = 0;
        int xPos = -1, yPos = -1;
        public HistForm(double[] arrLX, double[] arrLY, double[,] matrZ, int[,] matrC)
        {
            InitializeComponent();
            this.arrLX = new float[arrLX.Length];
            this.arrLY = new float[arrLY.Length];
            this.matrZ = new float[matrZ.GetLength(0), matrZ.GetLength(1)];
            this.matrC = matrC;
            for (int i = 0; i < matrZ.GetLength(0); i++)
            {
                this.arrLX[i] = (float)arrLX[i];
                for (int j = 0; j < matrZ.GetLength(1); j++)
                {
                    this.arrLY[i] = (float)arrLY[i];
                    this.matrZ[i, j] = (float)matrZ[i, j];                                        
                }
            }
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
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.LightBlue, 1.0f, 0);
            SetupCamera();
            List<CustomVertex.PositionColored> listV = new List<CustomVertex.PositionColored>();
            List<CustomVertex.PositionColored> listVL = new List<CustomVertex.PositionColored>();
            float xSum = 0, ySum = 0;
            for (int i = 0; i < arrLX.Length; i++)
                xSum += arrLX[i];
            for (int i = 0; i < arrLY.Length; i++)
                ySum += arrLY[i];
            float x0 = 0, y0 = 0, zMax = 0;
            for (int i = 0; i < matrZ.GetLength(0); i++)
                for (int j = 0; j < matrZ.GetLength(1); j++)
                    if (matrZ[i, j] > zMax)
                        zMax = matrZ[i, j];
            for (int i = 0; i < arrLX.Length; i++)
            {
                float dx = arrLX[i] / xSum;
                y0 = 0;
                for (int j = 0; j < arrLY.Length; j++)
                {
                    float dy = arrLY[j] / ySum, dz = -matrZ[i, j] / zMax;
                    CustomVertex.PositionColored[] arrV = new CustomVertex.PositionColored[]
                    {
                        new CustomVertex.PositionColored(x0, y0, 0, Color.DarkGray.ToArgb()),
                        new CustomVertex.PositionColored(x0 + dx, y0, 0, Color.Gray.ToArgb()),
                        new CustomVertex.PositionColored(x0 + dx, y0 + dy, 0, Color.DarkGray.ToArgb()),
                        new CustomVertex.PositionColored(x0, y0 + dy, 0, Color.Gray.ToArgb()),
                        new CustomVertex.PositionColored(x0, y0, dz, matrC[i, j]),
                        new CustomVertex.PositionColored(x0 + dx, y0, dz, Color.Gray.ToArgb()),
                        new CustomVertex.PositionColored(x0 + dx, y0 + dy, dz, matrC[i, j]),
                        new CustomVertex.PositionColored(x0, y0 + dy, dz, matrC[i, j])
                    };
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
                    int[] arrI = { 0, 4, 1, 1, 4, 5, 1, 5, 2, 2, 5, 6, 2, 6, 7, 2, 7, 3, 0, 3, 7, 0, 7, 4, 0, 2, 3, 0, 1, 2, 7, 6, 5, 7, 5, 4 };
                    int[] arrIL = { 0, 1, 1, 2, 2, 3, 3, 0, 0, 4, 1, 5, 2, 6, 3, 7, 4, 5, 5, 6, 6, 7, 7, 4 };
                    for (int k = 0; k < arrI.Length; k++)
                        listV.Add(arrV[arrI[k]]);
                    for (int k = 0; k < arrIL.Length; k++)
                        listVL.Add(arrVL[arrIL[k]]);
                    y0 += dy;
                }
                x0 += dx;
            }
            device.BeginScene();
            device.VertexFormat = CustomVertex.PositionColored.Format;
            device.RenderState.Lighting = false;
            device.DrawUserPrimitives(PrimitiveType.TriangleList, listV.Count / 3, listV.ToArray());
            device.DrawUserPrimitives(PrimitiveType.LineList, listVL.Count / 2, listVL.ToArray());
            int lc2 = Color.Black.ToArgb();
            CustomVertex.PositionColored[] arrVL2 = new CustomVertex.PositionColored[]
            {
                new CustomVertex.PositionColored(0, 0, 0, lc2),
                new CustomVertex.PositionColored(1.2f, 0, 0, lc2),
                new CustomVertex.PositionColored(0, 0, 0, lc2),
                new CustomVertex.PositionColored(0, 1.2f, 0, lc2),
                new CustomVertex.PositionColored(0, 0, 0, lc2),
                new CustomVertex.PositionColored(0, 0, -1, lc2)
            };
            device.DrawUserPrimitives(PrimitiveType.LineList, 3, arrVL2);
            device.EndScene();
            device.Present();
        }
        private void SetupCamera()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 20, 1, 1, 100);
            device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, -20), new Vector3(), new Vector3(0, 1, 0));
            device.Transform.World = Matrix.RotationZ(xRot);
            device.Transform.World *= Matrix.RotationX(yRot);
            device.RenderState.CullMode = Cull.None;            
        }
        void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (xPos > 0 && yPos > 0)
            {
                xRot += 0.07f * (e.X - xPos);
                yRot += 0.07f * (e.Y - yPos);
            }
            xPos = e.X;
            yPos = e.Y;
            Invalidate();
        }
    }
}
