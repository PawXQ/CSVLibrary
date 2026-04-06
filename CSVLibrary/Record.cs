using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVLibrary
{
    internal class Record
    {
        public string target { get; set; }
        public string type { get; set; }
        public string payment { get; set; }

        public string detail { get; set; }
        public string money { get; set; }
    }
}
