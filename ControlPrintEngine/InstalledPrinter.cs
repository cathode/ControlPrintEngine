using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPrintEngine
{
    internal class InstalledPrinter
    {
        internal InstalledPrinter()
        {
        }

        public string Name { get; set; }

        public string Status { get; set; }

        public bool IsDefault { get; set; }
        
        public bool IsNetworkPrinter { get; set; }
    }
}
