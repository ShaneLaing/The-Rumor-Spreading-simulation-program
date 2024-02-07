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
    public partial class ChartView : Form
    {
        
        public ChartView()
        {
            InitializeComponent();
        }

        private void Scottplot_Load(object sender, EventArgs e)
        {
            
        }

        public void FillChartSpeed(double[] xs, double[] ys)
        {
            formsPlot1.Plot.Clear();
            formsPlot1.Plot.AddScatter(xs, ys);

            formsPlot1.Refresh();

            
        }

        public void FillChartFill(double[] xf, double[] yf)
        {
            formsPlot2.Plot.Clear();
            formsPlot2.Plot.AddScatter(xf, yf);

            formsPlot2.Refresh();


        }

        public static void ShowGraph(double[] xs, double[] ys, double[] xf, double[] yf)
        {
            using (ChartView graph = new ChartView())
            {
                graph.FillChartSpeed(xs, ys);

                graph.FillChartFill(xf, yf);
                graph.ShowDialog();

                
            }
        }

    }
}
