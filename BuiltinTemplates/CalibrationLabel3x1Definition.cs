using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlPrintEngine.BuiltinTemplates
{
    public class CalibrationLabel3x1Definition : IPrintDocument
    {
        public string Name => "Calibration Label, 3x1";

        public PageOrientation Orientation => PageOrientation.Portrait;

        public PrintMedia Stock => PrintMedia.ThermalLabel3x1;

        public UserControl CreateControl(object data = null)
        {
            return new CalibrationLabel3x1Control();
        }
    }
}
