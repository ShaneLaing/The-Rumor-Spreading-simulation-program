using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RumerSpreading_ver0
{
    public partial class XYGrid : UserControl
    {

        public int Dotsize { get; set; }


        public Color[,] GridData { get; private set; } = null;

        public bool HasgridData =>
            GridData is object &&
            GridData.GetLength(0) > 0 &&
            GridData.GetLength(1) > 0 ;

        
        public XYGrid()
        {
            InitializeComponent();

            //this.SetStyle(ControlStyles.AllPaintingInWmPaint|
            //    ControlStyles.UserPaint|
            //    ControlStyles.OptimizedDoubleBuffer, true);

            // Like...completely own this control.
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.UserPaint
                , true);
            this.DoubleBuffered = true;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            e.Graphics.Clear(BackColor);
        }

        public void UpdateGrid(Color[,] arrGrid)
        {
            GridData = arrGrid;
            this.Invalidate();
        }

        private void XYGrid_paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(BackColor);

            if (!HasgridData)
                return;

            Random random = new Random();

            var dotSize = Dotsize;
            var dotGap = Dotsize;

            using (var bmp = new Bitmap(this.Width, this.Height))
            using (var gfx = Graphics.FromImage(bmp))
            {
                for (int row = 0; row < GridData.GetLength(0); row++)
                {
                    for (int column = 0; column < GridData.GetLength(1); column++)
                    {
                        var color = GridData[row, column];
                        (int x, int y) = (row * dotGap, column * dotGap);
                        var rect = new Rectangle(x, y, dotSize, dotSize);

                        gfx.FillRectangle(new SolidBrush(color), rect);


                    }
                }

                g.DrawImage(bmp, 0, 0);
            }
        }

        private void XYGrid_Load(object sender, EventArgs e)
        {

        }
    }
}
