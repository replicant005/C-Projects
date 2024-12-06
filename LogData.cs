//Author : mehak kapur 
//date created : 2/12/2024
//description : this is logData class that consists of data class which further consists of properties which further will be used to access  items in list

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
