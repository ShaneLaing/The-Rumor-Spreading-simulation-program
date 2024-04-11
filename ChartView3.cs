using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using Newtonsoft.Json.Linq;
using ScottPlot;
using ScottPlot.Drawing.Colormaps;
using ScottPlot.Palettes;
using ScottPlot.Renderable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace RumerSpreading.Ver1
{
    public partial class ChartView3 : Form
    {
        private Axis yAxis2;
        private GraphAnalyzingSettings _graphAnalyzingSettings;
        public GraphAnalyzingSettings GraphAnalyzingSettings
        {
            get => _graphAnalyzingSettings;
            set
            {
                _graphAnalyzingSettings = value;

                SMA_Radious.Text = _graphAnalyzingSettings?.r_SMA.ToString();

            }
        }

        //public void SetGraphAnalyzingSettings(GraphAnalyzingSettings settings)
        //{
        //    _graphAnalyzingSettings = settings;
        //}

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

        public void FillChart(
            
            params (double[] xs, double[] ys, bool isY2)[] values)
        {
            this.formsPlot2.Plot.Clear();
            foreach (var value in values)
            {
                if (value.xs?.Length > 0 && value.ys?.Length > 0)
                {
                    var sp = this.formsPlot2.Plot.AddScatter(
                        value.xs, value.ys);

                    if (value.isY2)
                    {
                        sp.YAxisIndex = this.yAxis2.AxisIndex;
                    }
                }

            }
            this.formsPlot2.Refresh();
        }

        public static void ShowGraphIdx3(GraphAnalyzingSettings graphAnalyzingSettings,
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

                graph.GraphAnalyzingSettings = graphAnalyzingSettings.Copy();

                graph.FillChart(values);

                graph.ShowDialog();



            }
        }

        private void ChartView3_Load_1(object sender, EventArgs e)
        {

        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {

        }

        private void CurveFitting_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Statistic_Click(object sender, EventArgs e)
        {
            RunStatisticFunction();

        }

        public void RunStatisticFunction()
        {
            this.GraphAnalyzingSettings.checkPoltRegression = CurveFitting.Checked;
            this.GraphAnalyzingSettings.checkMV_PrimeValues = MV_PrimeValues.Checked;

            var result = SimulationResult.GraphAnalyzing(this.GraphAnalyzingSettings);

            this.FillChart(

                (result.xValues, result.yValues, false),
                (result.xValues, result.yValues3, false),
                //(result.xValues, result.yValues3_5, false),
                (result.xValues, result.yValues4, false),
                //(result.xValues, result.curv_values, true)
                (result.xValues, result.Var_yPrime, true)

                //(xValues, MV_yPrimeValues.ToArray(), true)
                //(xValues, MV_yDoublePrimeValues.ToArray(), true)

                );
        }
        
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            try
            {
                int r_SMA = int.Parse(SMA_Radious.Text);

                if (r_SMA >= 2 && r_SMA <= 100)
                {                  
                    this.GraphAnalyzingSettings.r_SMA = Convert.ToInt32(SMA_Radious.Text);
                }


                else if (r_SMA > 100 || r_SMA < 2)
                {
                    MessageBox.Show("Please enter an available number.");
                    //SMA_Radious.Clear();
                }


            }
            catch (FormatException)
            {
                
                SMA_Radious.Clear();
            }
            
        }

        private void SMA_Radious_TextChanged(object sender, EventArgs e)
        {

        }

        private void MV_PrimeValues_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
