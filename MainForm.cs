using RumerSpreading_ver0;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using ScottPlot.Statistics;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

using Dasync.Collections;


namespace RumerSpreading.Ver1
{
    public partial class RumerSpreading : Form
    {

        private int[,] _gridData = null;
        public bool loop = true;
        private Dictionary<Point, double> _thresholdValue = new Dictionary<Point, double>();

        private XYGrid grid;
        public int sizeNum = 70;
        public double densityNum = 0.33;
        public int accountNum = 4;
        public int radNum = 3;
        public double tolerNum = 0.01;
        public int LoopTimesMin =100;

        public int young = 30;
        public int strong = 25;
        public int old = 45;

        public RumerSpreading()
        {
            InitializeComponent();



            grid = new XYGrid();
            grid.Dock = DockStyle.Fill;
            grid.Dotsize = 600 / sizeNum;
            this.panel2.Controls.Add(grid);

            Rad.Text = radNum.ToString();
            Account.Text = accountNum.ToString();
            Society.Text = sizeNum.ToString();
            Density.Text = (1 - densityNum).ToString();
            Society.Text = sizeNum.ToString();
            Age.Text = ($"{young}, {strong}, {old}").ToString();
            Tolerance.Text = tolerNum.ToString();
        }

        static double RandomNormal(Random random, double mean, double standardDeviation)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
            return mean + standardDeviation * z;
        }

        private void Rad_TextChanged(object sender, EventArgs e)
        {

            try
            {
                int RadNum = int.Parse(Rad.Text);

                if (RadNum >= 1 && RadNum <= 4)
                {
                    radNum = RadNum - 1;
                    DrawGrid();

                }

                else if (RadNum > 4 || RadNum < 1)
                {
                    MessageBox.Show("Please enter an available number.");
                    Rad.Clear();
                }

            }
            catch (FormatException)
            {

                Rad.Clear();
            }

        }

        private void Density_TextChanged(object sender, EventArgs e)
        {

            try
            {
                double DensityNum = double.Parse(Density.Text);

                if (DensityNum >= 0 && DensityNum <= 1)
                {
                    densityNum = 1 - DensityNum;
                    DrawGrid();

                }


                else if (DensityNum > 1 || DensityNum < 0)
                {
                    MessageBox.Show("Please enter an available number.");
                    Society.Clear();
                }

            }
            catch (FormatException)
            {

                Society.Clear();
            }
        }
        private void Account_TextChanged(object sender, EventArgs e)
        {

            try
            {
                int AccountNum = int.Parse(Account.Text);

                if (AccountNum >= 1 && AccountNum <= 1000)
                {
                    accountNum = AccountNum;
                    DrawGrid();

                }

                else if (AccountNum > 1000 || AccountNum < 1)
                {
                    MessageBox.Show("Please enter an available number.");
                    Account.Clear();
                }

            }
            catch (FormatException)
            {

                Account.Clear();
            }
        }
        private void Society_TextChanged(object sender, EventArgs e)
        {


            try
            {
                int SocietyNum = int.Parse(Society.Text);

                if (SocietyNum >= 5 && SocietyNum <= 600)
                {
                    sizeNum = SocietyNum;
                    DrawGrid();

                }

                else if (SocietyNum > 600)
                {
                    MessageBox.Show("Please enter an available number.");
                    Society.Clear();
                }
                else if (SocietyNum < 5);


            }
            catch (FormatException)
            {

                Society.Clear();
            }
        }

        private void Age_TextChanged(object sender, EventArgs e)
        {

        }


        public int[,] GetRandomGridData()
        {
            var random = new ThreadSafeRandom();

            _thresholdValue.Clear();
            int[,] gridData = new int[sizeNum, sizeNum];

            void initializeByRow(int row)
            {
                for (int column = 0; column < gridData.GetLength(1); column++)
                {
                    int percentage1 = young;
                    int percentage2 = strong;
                    int percentage3 = old;
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

            for (int i_0 = 0; i_0 < sizeNum * sizeNum * densityNum; i_0++)
            {
                while (true)
                {
                    int rowWhite = random.Next(0, sizeNum);
                    int columnWhite = random.Next(0, sizeNum);
                    if (!hsCoordinate.Contains(new { rowWhite, columnWhite }))
                    {
                        gridData[rowWhite, columnWhite] = 0;
                        hsCoordinate.Add(new { rowWhite, columnWhite });
                        break;
                    }
                }

            }

            for (int i_1 = 0; i_1 < accountNum; i_1++)
            {
                while (true)
                {
                    int rowRed = random.Next(0, sizeNum);
                    int columnRed = random.Next(0, sizeNum);

                    if (!hsCoordinate.Contains(new { rowRed, columnRed }))
                    {
                        int _radNum = random.Next(1, radNum + 1);

                        for (int rx = -_radNum; rx <= _radNum; rx++)
                        {
                            for (int ry = -_radNum; ry <= _radNum; ry++)
                            {
                                if (rowRed + rx >= 0 && columnRed + ry >= 0 && rowRed + rx < sizeNum && columnRed + ry < sizeNum /*&& gridData[rowRed + rx, columnRed + ry] != 0*/)
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


      
        public void DrawGrid()
        {
            var gridData = GetRandomGridData();
            DrawGrid(gridData);
        }
        public void DrawGrid(int[,] gridData)
        {

            Random random = new Random();
            grid.Dotsize = 600 / sizeNum;

            Color[,] ColorGridData = new Color[gridData.GetLength(0), gridData.GetLength(1)];// sizeNum, sizeNum];

            for (int row = 0; row < ColorGridData.GetLength(0); row++)
            {
                for (int column = 0; column < ColorGridData.GetLength(1); column++)
                {

                    var color = GetColor(gridData[row, column]);

                    ColorGridData[row, column] = color;
                }

            }

            grid.UpdateGrid(ColorGridData);
            

        }

        private Color GetColor(int value)
        {

            if (value == -1)
                return Color.FromArgb(0, 200, 154);

            else if (value == -2)
                return Color.FromArgb(255, 218, 100);

            else if (value == -3)
                return Color.FromArgb(60, 100, 250);

            else if (value == 1)
                return Color.FromArgb(150, 0, 0);

            return Color.White;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void RumerSpreading_Load(object sender, EventArgs e)
        {
            DrawGrid();
        }

        List<double> EverySpeed = new List<double>();
        List<double> EveryFill = new List<double>();

        
        private void btn_Run_Click(object sender, EventArgs e)
        {
            loop = false;

            var simulationJob = new SimulationJob();
            simulationJob.sizeNum = sizeNum;
            simulationJob.densityNum = densityNum;
            simulationJob.accountNum = accountNum;
            simulationJob.radNum = radNum;
            simulationJob.tolerNum = tolerNum;
            simulationJob.young = young;
            simulationJob.strong = strong;
            simulationJob.old = old;
            simulationJob.LoopTimesMin = LoopTimesMin;

            var simulationEngine = new SimulationEngine(simulationJob);
            simulationEngine.Run(DrawGrid, loop);
            if (simulationEngine.IsSuccess)
            {

            }

            return;




            bool stop = false;
            bool success = false;
            int lastMinusCount = 0;
            double TotalPop = sizeNum * sizeNum * (1 - densityNum);

            EverySpeed.Clear();
            EveryFill.Clear();
            while (!stop)
            {
                var (filteredGridData, NewMinusCount, NewOneCount, UsedValue) =
                ConvolutionFilter.ApplyFilter(_gridData, _thresholdValue);

                DrawGrid(filteredGridData);
                _gridData = filteredGridData;
                
                if (NewMinusCount + NewOneCount != 0 && UsedValue != 0)
                    EverySpeed.Add((double)NewOneCount / UsedValue);
                else
                    EverySpeed.Add(0d);

                EveryFill.Add(1 - (double)NewMinusCount / TotalPop);
                if (NewMinusCount == 0 /*|| NewMinusCount == lastMinusCount*/)
                {
                    stop = true;

                    double[] yValuesS = EverySpeed.ToArray();
                    double[] xValuesS = Enumerable.Range(0, EverySpeed.Count)
                        .Select(d => d + 1.0d).ToArray();



                    double[] yValuesF = EveryFill.ToArray();
                    double[] xValuesF = Enumerable.Range(0, EveryFill.Count)
                        .Select(d => d + 1.0d).ToArray();

                    ChartView.ShowGraph(xValuesS, yValuesS, xValuesF, yValuesF);


                }

                lastMinusCount = NewMinusCount;

                Application.DoEvents();
                System.Threading.Thread.Sleep(1);
            }

            if (success)
            {
                MessageBox.Show("Success");
            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {

        }

        private void Reset_Click(object sender, EventArgs e)
        {
            DrawGrid();
        }

        private void Enter_Click(object sender, EventArgs e)
        {

            string rateText = Age.Text;
            string[] numberStrings = rateText.Split(',');

            try 
            {
                if (Convert.ToInt32(numberStrings[0]) + Convert.ToInt32(numberStrings[1]) + Convert.ToInt32(numberStrings[2]) == 100)
                {
                    young = Convert.ToInt32(numberStrings[0]);
                    strong = Convert.ToInt32(numberStrings[1]);
                    old = Convert.ToInt32(numberStrings[2]);
                    DrawGrid();
                }
               
            }
            catch
            {
                MessageBox.Show("Please enter an available number.");
            }
               

        }

        private void Tolerance_TextChanged(object sender, EventArgs e)
        {
            string imputToler = Tolerance.Text;


            if (double.TryParse(imputToler, out _))
            {
                double TolerNum = double.Parse(Tolerance.Text);
                tolerNum = TolerNum;

            }
            else if (imputToler == null) ;

            else
            {
                Society.Clear();
            }

        }

        

        List<double> ySigmaList = new List<double>();
        List<double> ySigmaListAverage = new List<double>();

        static bool IsExpectedFormat(string title, string expectedPrefix)
        {

            return title.StartsWith(expectedPrefix);
        }



        private List<double> ReadJobValues()
        {

            var jobResultRefinedFolderPath = Path.Combine(Application.StartupPath, "JobResult_refined");

            if (!Directory.Exists(jobResultRefinedFolderPath))
                Directory.CreateDirectory(jobResultRefinedFolderPath);

            var jobResultFolderPath = GetJobResultFolderPath();
            var jobResultFiles = Directory
                .GetFiles(jobResultFolderPath)
                .OrderBy(d => d)
                .ToList();

            var RefinedFolderPath = GetRefinedFolderPath();
            var RefinedFiles = Directory
                .GetFiles(RefinedFolderPath)
                .OrderBy(d => d)
                .ToList();

            var firstJobResultFile = jobResultFiles.FirstOrDefault();
            var firstRefinedFile = RefinedFiles.FirstOrDefault();

            var found = false;
            if (firstJobResultFile is object && firstRefinedFile is object)
            {
                var jobResult = SimulationResult.Load(firstJobResultFile);
                var refined = SimulationResult.LoadJob(firstRefinedFile);

                found = jobResult.Job.Title == refined.Title;
            }

            if (!found && !(firstRefinedFile is object && firstJobResultFile is null))
            {

                
                var retvalue = new List<double>(new double[jobResultFiles.Count]);
                                
                var lastResult = SimulationResult.Load(jobResultFiles.Last());

                double LoopTimesMin = lastResult.Job.LoopTimesMin;
                double tolerNum = lastResult.Job.tolerNum;
                double sizeNum = lastResult.Job.sizeNum;


                string expectedPrefix = $"{sizeNum},{tolerNum},{LoopTimesMin}";

                Parallel.For(0, jobResultFiles.Count, (idx) => {
                    var file = jobResultFiles[idx];
                    var result = SimulationResult.Load(file);
                    retvalue[idx] = result.JobValue;


                    if (!IsExpectedFormat(result.Job.Title, expectedPrefix))
                    {

                        result.Job.Title = expectedPrefix;

                        string updatedJsonContent = JsonConvert.SerializeObject(result, Formatting.Indented);


                        File.WriteAllText(file, updatedJsonContent);
                    }


                });

                SimulationResult.ComputerizeFunction(LoopTimesMin, tolerNum, sizeNum, retvalue);
                return retvalue;
            }
            else
            {
                
                var retvalue = new List<double>(new double[RefinedFiles.Count]);

                var lastResult = SimulationResult.LoadJob(RefinedFiles.Last());


                double tolerNum = lastResult.tolerNum;

                Parallel.For(0, RefinedFiles.Count, (idx) => {
                    var file = RefinedFiles[idx];
                    var result = SimulationResult.LoadJob(file);
                    retvalue[idx] = result.Refined_result;

                });

                SimulationResult.GraphAnalyzing(retvalue, tolerNum);
                return retvalue;
            }


            //foreach (var file in jobResultFiles)
            //{
            //    var result = SimulationResult.Load(file);
            //    retvalue.Add(result.JobValue);

            //    LoopTimesMin = result.Job.LoopTimesMin;
            //    tolerNum = result.Job.tolerNum;
            //}
#if false 
            var retvalue = new List<double>();
            double LoopTimesMin = 0;
            double tolerNum = 0;
            foreach (var file in jobResultFiles)
            {
                var result = SimulationResult.Load(file);
                retvalue.Add(result.JobValue);

                LoopTimesMin = result.Job.LoopTimesMin;
                tolerNum = result.Job.tolerNum;
            }
#endif
            
        }



        public void RefinedComputerizeFunction(double timesNum, double tolerNum, double sizeNum, List<double> retvalue)
        {
            double densityNum = 1;

            string jobResultFolderPath = GetRefinedFolderPath();

            List<Tuple<int, double>> varList = new List<Tuple<int, double>>();

            for (int i = 0; i < (int)Math.Floor(1 / tolerNum); i++)
            {
                varList.Add(new Tuple<int, double>(i, densityNum));

                densityNum -= tolerNum;
            }

            ParallelOptions parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = (int)Math.Ceiling(Environment.ProcessorCount * 0.75)
            };


            Parallel.For(0, varList.Count, i =>
            {
                var item = varList[i];

                var jobData = new SimulationJob
                {
                    Title = $"{sizeNum},{tolerNum},{timesNum}",
                    Refined_result = retvalue[i],
                    tolerNum = tolerNum
                    
                };


                string RefinedJson = JsonConvert.SerializeObject(jobData, Formatting.Indented);
                string fileName = (i + 1).ToString("D9") + ".json";
                string RefinedFilePath = Path.Combine(jobResultFolderPath, fileName);

                File.WriteAllText(RefinedFilePath, RefinedJson);


            });

            SimulationResult.GraphAnalyzing(retvalue, tolerNum);
        }

        private async void StudyRunning_Click(object sender, EventArgs e)
        {
            var jobFolderPath = GetJobFolderPath();
            var jobFiles = Directory.GetFiles(jobFolderPath);


            var jobResultFolderPath = GetJobResultFolderPath();

            loop = true;
            this.progressBar1.Maximum = jobFiles.Length;

            var processedCount = 0;

            updateProgress();

            var maxDegreeOfParallelism = (int)Math.Ceiling(Environment.ProcessorCount * 0.75);
#if DEBUG
            maxDegreeOfParallelism = 1;
#endif
            

            if (chkUseDistribution.Checked)
            {
                await jobFiles.ParallelForEachAsync(
                async jobFile =>
                {
                    var job = JsonConvert.DeserializeObject<SimulationJob>(File.ReadAllText(jobFile));

                    var jobResultFileName = Path.GetFileNameWithoutExtension(jobFile) + ".retvalue.json";
                    var jobResultFilePath = Path.Combine(jobResultFolderPath, jobResultFileName);

                    var cmdArgs = new string[] {
                    jobFile,
                    jobResultFilePath
                    };

                    var cmd = Application.ExecutablePath;

                    var status = await ShellCommandExecutor.RunAsync(cmd, string.Join(" ", cmdArgs), traceOutputPipe);
                    Interlocked.Increment(ref processedCount);
                    updateProgress();
                },
                //maxDegreeOfParallelism: maxDegreeOfParallelism,
                cancellationToken: default);

            }
            else
            {
                Parallel.ForEach(jobFiles,
                    new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism },
                    jobFile =>
                //foreach (var jobFile in jobFiles)
                {


                    var job = JsonConvert.DeserializeObject<SimulationJob>(File.ReadAllText(jobFile));


                    var simulationEngine = new SimulationEngine(job);

                    simulationEngine.Run(DrawGrid, loop);

                    //var initiative = SimulationEngine.Load(jobFile);
                    //densityNum = initiative.JobData.densityNum;
                    if (simulationEngine.IsSuccess)
                    {
                        var jobResultFileName = Path.GetFileNameWithoutExtension(jobFile) + ".retvalue.json";
                        var jobResultFilePath = Path.Combine(jobResultFolderPath, jobResultFileName);

                        simulationEngine.Result.Save(jobResultFilePath);



                        File.Delete(jobFile);

                    }



                });
            }
            ReadJobValues();

            //if (MessageBox.Show("Job files are finished. Open folder?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{
            //    OpenFolderInExplorer(jobResultFolderPath);

            //}

            return;

            void traceOutputPipe(string text)
            {
                Trace.WriteLine(text);
            }
            
            void fakeDrawGrid(int[,] gridData)
            {
                //do nothing
            }

            void updateProgress()
            {

                this.Invoke(new Action(() => {

                    this.progressBar1.Value = processedCount;
                    this.progressBar1.Update();

                    this.lblSimulationProgress.Text = $"Processed: {processedCount}, Total: {this.progressBar1.Maximum}";

                }));

            }


        }

        private void StudyRunning_Click_bak(object sender, EventArgs e)
        {


            var jobFolderPath = GetJobFolderPath();
            var jobFiles = Directory.GetFiles(jobFolderPath);
            

            var jobResultFolderPath = GetJobResultFolderPath();

            loop = true;
            this.progressBar1.Maximum = jobFiles.Length;

            var processedCount = 0;

            updateProgress();

            Parallel.ForEach(jobFiles, jobFile =>
            //foreach (var jobFile in jobFiles)
            {
                

                var job = JsonConvert.DeserializeObject<SimulationJob>(File.ReadAllText(jobFile));

                
                var simulationEngine = new SimulationEngine(job);

                simulationEngine.Run(fakeDrawGrid, loop);

                Interlocked.Increment(ref processedCount);
                updateProgress();

                //var initiative = SimulationEngine.Load(jobFile);
                //densityNum = initiative.JobData.densityNum;
                if (simulationEngine.IsSuccess)
                {
                    var jobResultFileName = Path.GetFileNameWithoutExtension(jobFile) + ".retvalue.json";
                    var jobResultFilePath = Path.Combine(jobResultFolderPath, jobResultFileName);

                    simulationEngine.Result.Save(jobResultFilePath);

                    

                    File.Delete(jobFile);

                }

                

            });

            ReadJobValues();

            if (MessageBox.Show("Job files are finished. Open folder?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OpenFolderInExplorer(jobResultFolderPath);

            }

            return;



            void fakeDrawGrid(int[,] gridData)
            {
                //do nothing
            }

            void updateProgress()
            {

                this.Invoke(new Action(() => {
                    Application.DoEvents();
                    this.progressBar1.Value = processedCount;
                    this.progressBar1.Update();

                    this.lblSimulationProgress.Text = $"Processed: {processedCount}, Total: {this.progressBar1.Maximum}";

                }));

            }


        }

        public string GetRefinedFolderPath()
        {
            return System.IO.Path.Combine(Application.StartupPath, "JobResult_refined");
        }
        public string GetJobFolderPath()
        {
            return System.IO.Path.Combine(Application.StartupPath, "JobFolder");
        }

        public string GetJobResultFolderPath()
        {
            var path = System.IO.Path.Combine(Application.StartupPath, "JobResult");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
        
        public void CreateJobFiles(int sizeNum, double densityNum, int accountNum, int radNum, double tolerNum, int young, int strong, int old, int LoopTimesMin)
        {
            List<SimulationJob> jobs = new List<SimulationJob>();

            string jobFolderPath = GetJobFolderPath();

            


            if (Directory.Exists(jobFolderPath))
            {
                if (MessageBox.Show("folder already exists. Delete primitive JobFolder and create new JobFolder? (\"Yes\" to delete all job files the create new)", "confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                else
                {
                    Directory.Delete(jobFolderPath, true);
                    Directory.Delete(GetJobResultFolderPath(), true);
                }
            }

            DirectoryInfo JobFolder = Directory.CreateDirectory(jobFolderPath);

             decimal D_densityNum = 1 - (decimal)tolerNum;

         

            List<Tuple<int, double>> varList = new List<Tuple<int, double>>();

            //int numberOfIterations = (int)Math.Floor(LoopTimesMin / tolerNum)+(int)LoopTimesMin;

            for (int i = 0; i < (int)Math.Floor(1 / tolerNum); i++)
            {
                for (int j = 1; j <= LoopTimesMin; j++)
                {
                    varList.Add(new Tuple<int, double>(j, (double)D_densityNum));
                }

                D_densityNum -= (decimal)tolerNum;
            }

            ParallelOptions parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = (int)Math.Ceiling(Environment.ProcessorCount * 0.75)
            };


            Parallel.For(0, varList.Count, i =>
            {
                var item = varList[i];

                var jobData = new SimulationJob
                {
                    Title = $"{sizeNum},{tolerNum},{LoopTimesMin}",
                    sizeNum = sizeNum,
                    densityNum = item.Item2,
                    accountNum = accountNum,
                    radNum = radNum,
                    tolerNum = tolerNum,
                    young = young,
                    strong = strong,
                    old = old,
                    LoopTimesMin = LoopTimesMin,
                };

                
                string jobJson = JsonConvert.SerializeObject(jobData, Formatting.Indented);
                string fileName = (i + 1).ToString("D9") + ".json";
                string jobFilePath = Path.Combine(jobFolderPath, fileName);

                File.WriteAllText(jobFilePath, jobJson);


            });


            if (MessageBox.Show("Job files created. Open job folder?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                OpenFolderInExplorer(jobFolderPath);
               
            }

        }

        public void OpenFolderInExplorer(string folderPath)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = folderPath,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        private void CreateFold_Click(object sender, EventArgs e)
        {

            CreateJobFiles(sizeNum, densityNum, accountNum, radNum, tolerNum, young, strong, old, LoopTimesMin);

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Statistic_Click(object sender, EventArgs e)
        {
            ReadJobValues();
        }
    }
}
