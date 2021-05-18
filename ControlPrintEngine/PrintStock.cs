using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ControlPrintEngine
{
    public class PrintStock
    {
        /// <summary>
        /// Gets the page size (in WPF units) of the label.
        /// </summary>
        public Size PageSize { get; internal set; }
        
        /// <summary>
        /// Gets the page orientation.
        /// </summary>
        public PageOrientation Orientation { get; internal set; }
    }
}
