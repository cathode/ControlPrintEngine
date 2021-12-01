using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Printing;

namespace ControlPrintEngine
{
    /// <summary>
    /// Provides a simplified wrapper around a <see cref="System.Printing.PrintQueue"/> 
    /// </summary>
    public class PrintQueueWrapper
    {
        internal PrintQueueWrapper(PrintQueue pq)
        {
            this.BackendQueue = pq;
        }

        public string PrinterName { get { return this.BackendQueue?.Name; } }

        public string Status { get; set; }

        public bool IsDefault { get; set; }

        public bool IsNetworkPrinter { get; set; }

        public PrintQueue BackendQueue { get; private set; }
    }
}
