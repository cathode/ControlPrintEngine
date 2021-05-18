using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ControlPrintEngine.BuiltinTemplates
{
    public class CalibrationLabel3x1Definition : IPrintDocumentDefinition
    {
        public string Name
        {
            get
            {
                return "Calibration Label, 3x1";
            }
        }

        public PageOrientation Orientation
        {
            get
            {
                return PageOrientation.Portrait;
            }
        }

        public Size PageSize
        {
            get
            {
                return new Size(3.0 * 96, 1.0 * 96);
            }
        }

        public UserControl CreateControl(object data = null)
        {
            return new CalibrationLabel3x1Control();
        }
    }
}
