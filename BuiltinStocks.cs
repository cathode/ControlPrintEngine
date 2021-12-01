using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Printing;

namespace ControlPrintEngine
{
    public static class BuiltinStocks
    {
        public static readonly PrintStock ThermalLabel3x1 = new PrintStock { PageSize = new Size(3.0 * 96, 1.0 * 96), Orientation = PageOrientation.Landscape };
        public static readonly PrintStock ThermalLabel4x2 = new PrintStock { PageSize = new Size(4.0 * 96, 2.0 * 96), Orientation = PageOrientation.Landscape };
        public static readonly PrintStock ThermalLabel4x6 = new PrintStock { PageSize = new Size(4.0 * 96, 6.0 * 96), Orientation = PageOrientation.Portrait };
       
    }
}
