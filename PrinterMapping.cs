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

        public PrintMediaType PrinterMediaType { get; internal set; }

        public IEnumerable<PrintMedia> PrintableMedia { get; internal set; }
    }


}
