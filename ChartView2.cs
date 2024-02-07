using ScottPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RumerSpreading.Ver1
{
    public partial class ChartView2 : Form
    {
        public ChartView2()
        {
            InitializeComponent();
        }

        private void ChartView2_Load(object sender, EventArgs e)
        {

        }

        public void FillChartIdx(double[] xSigma, double[] ySigmaValue)
        {
            formsPlot1.Plot.Clear();
            formsPlot1.Plot.AddScatter(xSigma, ySigmaValue);

            formsPlot1.Refresh();


        }
        
        public static void ShowGraphIdx(double[] xs, double[] ys)
        {

            using (ChartView2 graph = new ChartView2())
            {

                graph.FillChartIdx(xs, ys);
                graph.ShowDialog();

                
            }
        }

        private void InitializeComponent()
        {
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot1.Location = new System.Drawing.Point(0, 0);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(960, 465);
            this.formsPlot1.TabIndex = 0;
            this.formsPlot1.Load += new System.EventHandler(this.formsPlot1_Load);
            // 
            // ChartView2
            // 
            this.ClientSize = new System.Drawing.Size(960, 465);
            this.Controls.Add(this.formsPlot1);
            this.Name = "ChartView2";
            this.ResumeLayout(false);

        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {

        }
    }
}
