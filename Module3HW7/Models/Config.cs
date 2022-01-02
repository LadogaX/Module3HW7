using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module3HW7.Models
{
   public class Config
    {
        public string LogFolderPath { get; set; }

        public string BackUpFolderPath { get; set; }

        public int CountRecordsFlushBackUp { get; set; }
    }
}
