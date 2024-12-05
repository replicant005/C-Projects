using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningLog2024
{
    public static class LogData
    {
        public class Data
        {
            public int Id { get; set; }
            public DateTime EntryDate { get; set; }
            public int Wellness { get; set; }
            public int Quality { get; set; }
            public string Notes { get; set; }
        }
    }
}
