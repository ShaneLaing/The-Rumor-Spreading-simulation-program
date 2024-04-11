using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumerSpreading.Ver1
{
    public class GraphAnalyzingSettings
    {
        public List<double> Statistic_Sums;
        public double tolerNum;
        public int r_SMA;

        public bool checkPoltRegression = false;
        public bool checkMV_PrimeValues = false;

        public GraphAnalyzingSettings(List<double> Statistic_Sums, double tolerNum, int r_SMA) 
        { 
            this.Statistic_Sums = Statistic_Sums;
            this.tolerNum = tolerNum;
            this.r_SMA = r_SMA;
        }

        public GraphAnalyzingSettings Copy() 
        { 
            return JsonConvert.DeserializeObject<GraphAnalyzingSettings>(JsonConvert.SerializeObject(this));
        }
    }


}
