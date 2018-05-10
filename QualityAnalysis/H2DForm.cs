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
    public partial class H2DForm : Form
    {
        private Device device = null;
        double[] arrLX, arrLY;
        double[,] matrZ;
        int[,] matrC;
        public double zMax;
        public H2DForm(double[] arrLX, double[] arrLY, double[,] matrZ, int[,] matrC, double zMax)
        {
            InitializeComponent();
            this.arrLX = arrLX;
            this.arrLY = arrLY;
            this.matrZ = matrZ;
            this.matrC = matrC;
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
            try
            {
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.White, 1.0f, 0);
                SetupCamera();
                List<CustomVertex.PositionColored> listV = new List<CustomVertex.PositionColored>();
                List<CustomVertex.PositionColored> listVL = new List<CustomVertex.PositionColored>();
                float xSum = 0, ySum = 0;
                for (int i = 0; i < arrLX.Length; i++)
                    xSum += (float)arrLX[i];
                for (int i = 0; i < arrLY.Length; i++)
                    ySum += (float)arrLY[i];
                float x0 = 0, y0 = 0;
                zMax = 1;
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
                        float dy = (float)arrLY[j] / ySum, dz = -(float)(matrZ[i, j] / zMax);
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
                    new CustomVertex.PositionColored(0, 0, -1.2f, lc2)
                };
                device.DrawUserPrimitives(PrimitiveType.LineList, 3, arrVL2);
                CustomVertex.PositionTextured[] arrV3 = new CustomVertex.PositionTextured[]
                {
                    new CustomVertex.PositionTextured(0, 0.2f, 0, 0, 0),
                    new CustomVertex.PositionTextured(0, 0.2f, -1.8f, 0, 1),
                    new CustomVertex.PositionTextured(0, 1.2f, 0, 1, 0),
                    new CustomVertex.PositionTextured(0, 1.2f, -1.8f, 1, 1)
                };
                int a = 200;
                Bitmap bmp = new Bitmap(a, a);
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.White);
                g.DrawString("y2", new Font("Times New Roman", 30), Brushes.Black, a - 60, a - 50);
                bmp.RotateFlip(RotateFlipType.Rotate180FlipX);                
                Texture t1 = new Texture(device, bmp, 0, Pool.Managed);
                CustomVertex.PositionTextured[] arrV4 = new CustomVertex.PositionTextured[]
                {
                    new CustomVertex.PositionTextured(0.2f, 0, 0, 0, 0),
                    new CustomVertex.PositionTextured(0.2f, 0, -1.8f, 0, 1),
                    new CustomVertex.PositionTextured(1.2f, 0, 0, 1, 0),
                    new CustomVertex.PositionTextured(1.2f, 0, -1.8f, 1, 1)
                };
                bmp = new Bitmap(a, a);
                g = Graphics.FromImage(bmp);
                g.Clear(Color.White);
                g.DrawString("P", new Font("Times New Roman", 30), Brushes.Black, a - 50, 20);
                g.DrawString("y1", new Font("Times New Roman", 30), Brushes.Black, 0, a - 50);
                bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                Texture t2 = new Texture(device, bmp, 0, Pool.Managed);
                device.VertexFormat = CustomVertex.PositionTextured.Format;
                device.SetTexture(0, t1);                
                device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, arrV3);
                device.SetTexture(0, t2);
                device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, arrV4);
                device.EndScene();
                device.Present();
            }
            catch { }
        }
        void SetupCamera()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 20, 1, 1, 100);
            device.Transform.View = Matrix.LookAtLH(new Vector3(13, 17, -15), new Vector3(), new Vector3(0, 0, -1));
            //device.Transform.World = Matrix.RotationZ(xRot);
            //device.Transform.World *= Matrix.RotationX(yRot);
            device.RenderState.CullMode = Cull.None;            
        }
    }
}
