using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RumerSpreading_ver0;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Optimization.TrustRegion;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Skender.Stock.Indicators;
using MathNet.Numerics.Financial;
using System.Globalization;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.Differentiation;
using MathNet.Numerics.Interpolation;
using ScottPlot.Drawing.Colormaps;
using MoreLinq;



namespace RumerSpreading.Ver1
{

    public class SimulationEngine
    {
        
        public SimulationJob JobData { get; private set; }

        public static SimulationEngine Load(string filePath)
        {
            return JsonConvert.DeserializeObject<SimulationEngine>(File.ReadAllText(filePath));
        }
        public bool IsSuccess { get; private set; }
        
        public SimulationResult Result { get; private set; }


        private int[,] _gridData = null;
        private Dictionary<Point, double> _thresholdValue = new Dictionary<Point, double>();

        private XYGrid grid;

        List<double> EverySpeed = new List<double>();
        List<double> EveryFill = new List<double>();

        public SimulationEngine(SimulationJob job)
        {
            JobData = job;

            _gridData = GetRandomGridData();
        }



        public int[,] GetRandomGridData()
        {
            var random = new ThreadSafeRandom();

            _thresholdValue.Clear();
            int[,] gridData = new int[JobData.sizeNum, JobData.sizeNum];

            void initializeByRow(int row)
            {
                for (int column = 0; column < gridData.GetLength(1); column++)
                {
                    int percentage1 = JobData.young;
                    int percentage2 = JobData.strong;
                    int percentage3 = JobData.old;
                    int randomMinus = random.Next(1, 101);

                    if (randomMinus <= percentage1)
                    {
                        gridData[row, column] = -1;
                    }

                    else if (randomMinus <= (percentage1 + percentage2))
                    {
                        gridData[row, column] = -2;
                    }

                    else
                    {
                        gridData[row, column] = -3;
                    }
                }
            }

            var queryFlatten = Enumerable.Range(0, gridData.GetLength(0))
                .SelectMany(row => Enumerable.Range(0, gridData.GetLength(1)).Select(col => gridData[row, col]));

            Parallel.For(0, gridData.GetLength(0), (row) =>
            {
                initializeByRow(row);
            });
            var hsCoordinate = new HashSet<dynamic>();

            for (int i_0 = 0; i_0 < JobData.sizeNum * JobData.sizeNum * JobData.densityNum; i_0++)
            {
                while (true)
                {
                    int rowWhite = random.Next(0, JobData.sizeNum);
                    int columnWhite = random.Next(0, JobData.sizeNum);
                    if (!hsCoordinate.Contains(new { rowWhite, columnWhite }))
                    {
                        gridData[rowWhite, columnWhite] = 0;
                        hsCoordinate.Add(new { rowWhite, columnWhite });
                        break;
                    }
                }

            }

            for (int i_1 = 0; i_1 < JobData.accountNum; i_1++)
            {
                while (true)
                {
                    int rowRed = random.Next(0, JobData.sizeNum);
                    int columnRed = random.Next(0, JobData.sizeNum);

                    if (!hsCoordinate.Contains(new { rowRed, columnRed }))
                    {
                        int _radNum = random.Next(0, JobData.radNum + 1);

                        for (int rx = -_radNum; rx <= _radNum; rx++)
                        {
                            for (int ry = -_radNum; ry <= _radNum; ry++)
                            {
                                if (rowRed + rx >= 0 && columnRed + ry >= 0 && rowRed + rx < JobData.sizeNum && columnRed + ry < JobData.sizeNum /*&& gridData[rowRed + rx, columnRed + ry] != 0*/)
                                    gridData[rowRed + rx, columnRed + ry] = 1;
                            }
                        }
                        gridData[rowRed, columnRed] = 1;
                        hsCoordinate.Add(new { rowRed, columnRed });
                        break;
                    }
                }


            }

            for (int row = 0; row < gridData.GetLength(0); row++)
                for (int column = 0; column < gridData.GetLength(1); column++)
                    _thresholdValue.Add(new Point(row, column), RandomThreShold(new Point(row, column), gridData));

            var countZero = queryFlatten.Count(d => d == 0);
            var countMinus = queryFlatten.Count(d => d == -1 || d == -2 || d == -3);
            var countOne = queryFlatten.Count(d => d == 1);

            _gridData = gridData;
            return gridData;

        }

        private double RandomThreShold(Point ThresholdPoint, int[,] gridData)
        {

            Random random = new Random(Guid.NewGuid().GetHashCode());
            if (gridData[ThresholdPoint.X, ThresholdPoint.Y] == -1)
            {
                double YoungRandom = 0;
                double mean = 0.4;
                double standardDeviation = 0.1;
                YoungRandom = RandomNormal(random, mean, standardDeviation);
                while (YoungRandom <= 0.1 || YoungRandom >= 0.9)
                {
                    YoungRandom = RandomNormal(random, mean, standardDeviation);
                }
                return YoungRandom;
                //return random.NextDouble() * (0.99 - 0.01) + 0.01;
            }
            else if (gridData[ThresholdPoint.X, ThresholdPoint.Y] == -2)
            {
                double StrongRandom = 0;
                double mean = 0.3;
                double standardDeviation = 0.1;
                StrongRandom = RandomNormal(random, mean, standardDeviation);
                while (StrongRandom <= 0.1 || StrongRandom >= 0.5)
                {
                    StrongRandom = RandomNormal(random, mean, standardDeviation);
                }
                return StrongRandom;
                //return random.NextDouble() * (0.6 - 0.2) + 0.2;
            }
            else if (gridData[ThresholdPoint.X, ThresholdPoint.Y] == -3)
            {
                double OldRandom = 0;
                double mean = 0.25;
                double standardDeviation = 0.1;
                OldRandom = RandomNormal(random, mean, standardDeviation);
                while (OldRandom <= 0.05 || OldRandom >= 0.35)
                {
                    OldRandom = RandomNormal(random, mean, standardDeviation);
                }
                return OldRandom;
                //return random.NextDouble() * (0.3 - 0.05) + 0.05;
            }
            else
                return 0.0;
        }

        static double RandomNormal(Random random, double mean, double standardDeviation)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
            return mean + standardDeviation * z;
        }


        public void Run(Action<int[,]> drawData, bool ISloop)
        {
            Result = new SimulationResult
            { 
                Job = JobData
            };
            bool stop = false;
            bool success = false;
            int lastMinusCount = 0;
            int lastOneCount = -1;

            double TotalPop = JobData.sizeNum * JobData.sizeNum * (1 - JobData.densityNum);// sizeNum * sizeNum * (1 - densityNum);

            EverySpeed.Clear();
            EveryFill.Clear();
            while (!stop)
            {
                var (filteredGridData, NewMinusCount, NewOneCount, UsedValue) =
                ConvolutionFilter.ApplyFilter(_gridData, _thresholdValue);

                drawData?.Invoke(filteredGridData);
                _gridData = filteredGridData;

                if (lastOneCount == -1)
                {
                    lastMinusCount = NewMinusCount;
                    lastOneCount = NewOneCount;
                    continue;
                }
   
                if (NewMinusCount + NewOneCount != 0 && UsedValue != 0 && lastOneCount != 0 && NewMinusCount !=0)
                    EverySpeed.Add((double)NewOneCount / (double)lastOneCount -1);


                EveryFill.Add(1 - (double)NewMinusCount / TotalPop);
                if (NewMinusCount == 0 || NewMinusCount == lastMinusCount)
                {
                    stop = true;

                    double[] yValuesS = EverySpeed.ToArray();

                    if (ISloop == false)
                    {
                        
                        double[] xValuesS = Enumerable.Range(0, EverySpeed.Count)
                            .Select(d => d + 1.0d).ToArray();

                        double[] yValuesF = EveryFill.ToArray();
                        double[] xValuesF = Enumerable.Range(0, EveryFill.Count)
                            .Select(d => d + 1.0d).ToArray();

                        ChartView.ShowGraph(xValuesS, yValuesS, xValuesF, yValuesF);
                        break;
                    }

                    else if (ISloop == true)
                    {
                        Result.JobValue = EveryLoopValue(yValuesS);
                        break;
                    }


                }

                lastMinusCount = NewMinusCount;
                lastOneCount = NewOneCount;
                Application.DoEvents();
                System.Threading.Thread.Sleep(2);
            }

            IsSuccess = true;

        }

        public double EveryLoopValue(double[] ys)
        {
            //decimal sum = 0.0M;
            //foreach (double value in ys)
            //{
            //    sum += (decimal)value;
            //}
            //var mean = sum / ys.Length;

            //var squaredDiffSum = 0.0M;
            //foreach (double value in ys)
            //{
            //    var diff = (decimal)value - mean;
            //    squaredDiffSum += diff * diff;
            //}
            //var ySigma = (decimal)Math.Sqrt((double)squaredDiffSum / ys.Length);

            List<decimal> NewList = new List<decimal>();

            foreach (double value in ys)
            {
                NewList.Add((decimal)value);
            }
            //if (ySigma !=0)
            //{
                
            //}

            //if (ySigma != 0)
            //{
            //    foreach (double value in ys)
                    
            //        NewList.Add((((decimal)value - mean) / ySigma) * (((decimal)value - mean) / ySigma));
            //}
            //else
            //    foreach (double value in ys)
            //        NewList.Add(0);

            List<decimal> AbsSum = new List<decimal>();

            for (int i = 0; i < NewList.Count - 1; i++)
            {
                AbsSum.Add(Math.Abs(NewList[i] - NewList[i + 1]));
            }

            var JobValue = (double)AbsSum.Sum();

            return JobValue;

        }
        

    }


    public class SimulationResult
    {

        public double JobValue { get; set; }
        public SimulationJob Job { get; set; }

        public void Save(string filePath)
        {
            var jsonText = JsonConvert.SerializeObject(this);
            File.WriteAllText(filePath, jsonText);
        }

        public static SimulationResult Load(string filePath)
        {
            //using (var stream = File.OpenRead(filePath))
            using (var streamReader = new StreamReader(filePath))
            {
                return JsonConvert.DeserializeObject<SimulationResult>(streamReader.ReadToEnd());
            }
            //return JsonConvert.DeserializeObject<SimulationResult>(File.ReadAllText(filePath));
        }

        public static SimulationJob LoadJob(string filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            {
                return JsonConvert.DeserializeObject<SimulationJob>(streamReader.ReadToEnd());
            }
            //return JsonConvert.DeserializeObject<SimulationJob>(File.ReadAllText(filePath));
        }


        public static void ComputerizeFunction(double TimesNum, double TolerNum, double SizeNum, List<double> retvalue, int r_SMA)
        {
            bool checkPLotRegression = false;
            bool checkMV_PrimeValues = true;

            int timesNum = (int)TimesNum;
            List<double> sums = new List<double>();

            for (int i = 0; i < retvalue.Count; i += timesNum)
            {
                double sum = 0;
                for (int j = 0; j < timesNum && i + j < retvalue.Count; j++)
                {
                    sum += retvalue[i + j];
                }
                sums.Add((double)sum / timesNum);
            }

            var rumerspreading = new RumerSpreading();

            rumerspreading.RefinedComputerizeFunction(TimesNum, TolerNum, SizeNum, sums);


            GraphAnalyzing(sums, TolerNum, r_SMA, checkPLotRegression, checkMV_PrimeValues);

            //ChartView2.ShowGraphIdx(xValues, yValues);

        }

        public class GraphAnalyzingResult
        {
            public double[] xValues;
            public double[] yValues;
            public double[] yValues3;
            public double[] yValues3_5;
            public double[] yValues4;
            public double[] curv_values;
            public double[] Var_yPrime;

        }
        public static GraphAnalyzingResult GraphAnalyzing(GraphAnalyzingSettings graphAnalyzingSettings)
        {
            return GraphAnalyzing(
                graphAnalyzingSettings.Statistic_Sums,
                graphAnalyzingSettings.tolerNum,
                graphAnalyzingSettings.r_SMA,
                graphAnalyzingSettings.checkPoltRegression,
                graphAnalyzingSettings.checkMV_PrimeValues
                );
        }
 
        public static GraphAnalyzingResult GraphAnalyzing(List<double> sums, double TolerNum, int r_SMA, bool checkPlotRegression, bool checkMV_PrimeValues)
        {

            List<double> NewSums = new List<double>();

            foreach (double t in sums)
            {
                NewSums.Add((double)t / sums.Max());
            }

            double[] yValues = NewSums.ToArray();
            double[] xValues = Enumerable.Range(0, NewSums.Count).Select(d => (d + 1) * TolerNum).ToArray();


            var result = MovingAverage(NewSums, r_SMA);
            double[] yValues3 = result.ToArray();

            var yValues3_5 = new double[0];

            if (checkPlotRegression)
            {
                yValues3_5 = GuessPolyRegression(xValues, yValues3);
            }


            var yValues4 = MovingAverage(yValues3_5.ToList(), r_SMA).ToArray();

            var cs = CubicSpline.InterpolateAkimaSorted(xValues, yValues3);
            var yPrimeValues = xValues.Select(d => cs.Differentiate(d)).ToList();
            var MV_yPrimeValues = MovingAverage(yPrimeValues, r_SMA);

            var cs_2 = CubicSpline.InterpolateAkimaSorted(xValues, MV_yPrimeValues.ToArray());
            var yDoublePrimeValues = xValues.Select(d => cs_2.Differentiate(d)).ToList();
            var MV_yDoublePrimeValues = MovingAverage(yDoublePrimeValues, r_SMA);


            List<double> List_yPrime = MV_yPrimeValues.Select(x => Math.Atan(x)).ToList();

            List<double> Var_yPrime = new List<double>();

            if (checkMV_PrimeValues)
            {
                Var_yPrime = CalculateStandardDeviations(List_yPrime, windowSize: r_SMA);
            }


            var yPrimeAll = MV_yPrimeValues.Zip(MV_yDoublePrimeValues, (yPrime, yPrime2) => (yPrime, yPrime2));

            double[] curv_values = yPrimeAll.Select(d => Math.Log10(Math.Abs((d.yPrime2) / Math.Pow(1 + d.yPrime * d.yPrime, 1.5)))).ToArray();



            return new GraphAnalyzingResult
            {
                xValues = xValues,
                yValues = yValues,
                yValues3 = yValues3,
                yValues3_5 = yValues3_5,
                yValues4 = yValues4,
                curv_values = curv_values,
                Var_yPrime = Var_yPrime.ToArray(),
            
            };

            
        }


        public static List<double> MovingAverage(List<double> NewSums, int r_SMA)
        {
            
            List<double> sma = new List<double>();

            for (int i = 0; i < NewSums.Count; i++)
            {
                int startIndex = Math.Max(0, i - r_SMA);
                int endIndex = Math.Min(NewSums.Count - 1, i + r_SMA);

                double average = NewSums.GetRange(startIndex, endIndex - startIndex + 1).Average();

                sma.Add(average);
                                
            }

            return sma;

        }


        public static double[] GuessPolyRegression(double[] xValues, double[] yValues)
        {
            List<(double, int)> y_delta = new List<(double, int)>();

            double New_y_delta = 0;

            int ployRegression_delta = (int)(Math.Ceiling(xValues.Length / 20.0));

            for (int i = 0; i < xValues.Length; i += ployRegression_delta)
            {
                var polyRegression = Polynomial.Fit(xValues, yValues, order: i);

                var yValues_hat = xValues.Select(d => polyRegression.Evaluate(d)).ToArray();

                New_y_delta = 0;
                for (int j = 0; j < xValues.Length; j++)
                {
                    New_y_delta += (yValues[j] - yValues_hat[j]) * (yValues[j] - yValues_hat[j]);
                }

                y_delta.Add((Math.Sqrt(New_y_delta / xValues.Length), i));

                yValues_hat = new double[yValues_hat.Length];


            }

            int minIndex1 = y_delta.First(t => t.Item1 == y_delta.Min(x => x.Item1)).Item2;


            for (int i = Math.Max(0, minIndex1 - 2 * ployRegression_delta); i < Math.Min(minIndex1 + 2 * ployRegression_delta, xValues.Length); i++)
            {
                var polyRegression = Polynomial.Fit(xValues, yValues, order: i);

                var yValues_hat = xValues.Select(d => polyRegression.Evaluate(d)).ToArray();

                New_y_delta = 0;
                for (int j = 0; j < xValues.Length; j++)
                {
                    New_y_delta += (yValues[j] - yValues_hat[j]) * (yValues[j] - yValues_hat[j]);
                }

                y_delta.Add((Math.Sqrt(New_y_delta / xValues.Length), i));

                yValues_hat = new double[yValues_hat.Length];


            }
            int minIndex2 = y_delta.First(t => t.Item1 == y_delta.Min(x => x.Item1)).Item2;

            var polyRegression2 = Polynomial.Fit(xValues, yValues, order: minIndex2);
            var yValues4 = xValues.Select(d => polyRegression2.Evaluate(d)).ToArray();

            return yValues4;
        }

        static List<double> CalculateStandardDeviations(List<double> data, int windowSize)
        {
            List<double> standardDeviations = new List<double>();

            for (int i = 0; i < data.Count; i++)
            {
                int startIndex = Math.Max(0, i - windowSize);
                int endIndex = Math.Min(data.Count - 1, i + windowSize);

                List<double> windowData = data.GetRange(startIndex, endIndex - startIndex + 1);

                double standardDeviation = CalculateStandardDeviation(windowData);
                standardDeviations.Add(standardDeviation);
            }

            return standardDeviations;


        }
        static double CalculateStandardDeviation(List<double> data)
        {
            if (data.Count < 2)
                return 0;

            double mean = data.Average();
            double sumOfSquares = data.Sum(x => Math.Pow(x - mean, 2));
            double variance = sumOfSquares / (data.Count - 1);
            return Math.Sqrt(variance);
        }
    }
    
}




