using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RumerSpreading.Ver1
{
    public class SimulationJob
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int sizeNum { get; set; }

        public double densityNum { get; set; }
        public int accountNum { get; set; }
        public int radNum { get; set; }
        public double tolerNum { get; set; }
        public int young { get; set; }
        public int strong { get; set; }
        public int old { get; set; }

        public int LoopTimesMin { get; set; }

        public double Refined_result { get; set; }




    }
}
