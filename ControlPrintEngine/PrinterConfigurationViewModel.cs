using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPrintEngine
{
    internal class PrinterConfigurationViewModel
    {
        public List<InstalledPrinter> Printers { get; set; }

        public InstalledPrinter SelectedPrinter { get; set; }
    }
}
