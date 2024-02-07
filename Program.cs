using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RumerSpreading.Ver1
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                RunSimulation(args[0], args[1]);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new RumerSpreading());
            }
        }

        static void RunSimulation(string jobFile, string jobResultFilePath)
        {
            void traceLog(string msg)
            {
                Console.WriteLine($"[{DateTime.Now.ToString("MM-dd HH:mm:ss.ffff")}][{jobFile}] {msg}");
            }

            int runCount = 0;
            void DrawGrid(int[,] gridData)
            {
                //do nothing
                runCount++;
                traceLog($"run-count: {runCount}");

            }

            var jobText = File.ReadAllText(jobFile);
            traceLog($"jobText: {jobText}");
            var job = JsonConvert.DeserializeObject<SimulationJob>(jobText);

            var simulationEngine = new SimulationEngine(job);

            bool isLoop = true;

            simulationEngine.Run(DrawGrid, isLoop);

            traceLog($"simulationEngine.Run() finished, isSuccess: {simulationEngine.IsSuccess}");


            //var initiative = SimulationEngine.Load(jobFile);
            //densityNum = initiative.JobData.densityNum;
            if (simulationEngine.IsSuccess)
            {
                //var jobResultFileName = Path.GetFileNameWithoutExtension(jobFile) + ".retvalue.json";
                //var jobResultFilePath = Path.Combine(jobResultFolderPath, jobResultFileName);

                simulationEngine.Result.Save(jobResultFilePath);
//#if !DEBUG
                File.Delete(jobFile);
//#endif
            }
            traceLog($"Environment.Exit(0)");

            //Application.Exit();
            Environment.Exit(0);
        }
    }
}
