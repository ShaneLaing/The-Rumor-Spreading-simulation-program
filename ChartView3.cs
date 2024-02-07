using MathNet.Numerics;
using Newtonsoft.Json.Linq;
using ScottPlot;
using ScottPlot.Palettes;
using ScottPlot.Renderable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RumerSpreading.Ver1
{
    public partial class ChartView3 : Form
    {
        private Axis yAxis2;

        public ChartView3()
        {
            InitializeComponent();

            var plt = formsPlot2.Plot;
            yAxis2 = plt.AddAxis(Edge.Right);

        }


        

        public void FillChartIdx3(double[] Vx, double[] Vy)
        {

            formsPlot2.Plot.Clear();
            formsPlot2.Plot.AddScatter(Vx, Vy);

            formsPlot2.Refresh();
        }

        public void FillChartIdx3(
            double[] Vx1, double[] Vy1,
            double[] Vx2, double[] Vy2
            )
        {

            formsPlot2.Plot.Clear();
            formsPlot2.Plot.AddScatter(Vx1, Vy1);
            formsPlot2.Plot.AddScatter(Vx2, Vy2);

            formsPlot2.Refresh();
        }

        //static double EvaluatePolynomial(double x, double[] coefficients)
        //{
        //    double result = 0;

        //    for (int i = 0; i < coefficients.Length; i++)
        //    {
        //        result += coefficients[i] * Math.Pow(x, i);
        //    }

        //    return result;
        //}
        public static void ShowGraphIdx3(double[] xs, double[] ys)
        {

            using (ChartView3 graph = new ChartView3())
            {
                //double[] fittedValues = new double[xs.Length];

                //for (int i = 0; i < xs.Length; i++)
                //{
                //    double x = xs[i];
                //    double y = EvaluatePolynomial(x, p);
                //    fittedValues[i] = y;
                //}

                graph.FillChartIdx3(xs, ys);
                graph.ShowDialog();


            }
        }

        public static void ShowGraphIdx3(double[] xs1, double[] ys1,
            double[] xs2, double[] ys2)
        {

            using (ChartView3 graph = new ChartView3())
            {
                //double[] fittedValues = new double[xs.Length];

                //for (int i = 0; i < xs.Length; i++)
                //{
                //    double x = xs[i];
                //    double y = EvaluatePolynomial(x, p);
                //    fittedValues[i] = y;
                //}

                graph.FillChartIdx3(xs1, ys1, xs2, ys2);
                graph.ShowDialog();


            }
        }

        public static void ShowGraphIdx3(
            params (double[] xs, double[] ys, bool isY2)[] values)
        {

            using (ChartView3 graph = new ChartView3())
            {
                //double[] fittedValues = new double[xs.Length];

                //for (int i = 0; i < xs.Length; i++)
                //{
                //    double x = xs[i];
                //    double y = EvaluatePolynomial(x, p);
                //    fittedValues[i] = y;
                //}

                graph.formsPlot2.Plot.Clear();
                foreach (var value in values)
                {
                    var sp = graph.formsPlot2.Plot.AddScatter(
                        value.xs, value.ys);

                    if (value.isY2)
                    {
                        sp.YAxisIndex = graph.yAxis2.AxisIndex;
                    }

                }
                graph.formsPlot2.Refresh();

                graph.ShowDialog();



            }
        }

        private void ChartView3_Load_1(object sender, EventArgs e)
        {

        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {

        }
    }
}
