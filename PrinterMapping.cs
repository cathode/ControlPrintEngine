using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPrintEngine
{
    public class PrinterMapping
    {
        public string PrinterPath { get; set; }

        public PrintMediaType MediaType { get; set; }

        public IEnumerable<string> PrintableMediaNames { get; set; }

        public IEnumerable<PrintMedia> PrintableMedia { get; set; }


    }
}
