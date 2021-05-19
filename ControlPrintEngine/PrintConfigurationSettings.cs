using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPrintEngine
{
    public class PrintConfigurationSettings
    {
        public string PrinterPath { get; set; }

        public PrintQueueWrapper ConfiguredPrinter { get; set; }
    }
}
