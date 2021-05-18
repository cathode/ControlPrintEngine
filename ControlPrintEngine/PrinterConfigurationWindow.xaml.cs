using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Management;

namespace ControlPrintEngine
{
    /// <summary>
    /// Interaction logic for PrinterConfigurationWindow.xaml
    /// </summary>
    public partial class PrinterConfigurationWindow : Window
    {
        public PrinterConfigurationWindow()
        {
            InitializeComponent();
        }



        protected override void OnInitialized(EventArgs e)
        {
            var vm = new PrinterConfigurationViewModel();

            var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");

            vm.Printers = new List<InstalledPrinter>();

            foreach (var printer in printerQuery.Get())
            {
                var name = printer.GetPropertyValue("Name");
                var status = printer.GetPropertyValue("Status");
                var isDefault = printer.GetPropertyValue("Default");
                var isNetworkPrinter = printer.GetPropertyValue("Network");

                vm.Printers.Add(new InstalledPrinter { Name = name.ToString(), Status = status.ToString(), IsDefault = bool.Parse(isDefault.ToString()), IsNetworkPrinter = bool.Parse(isNetworkPrinter.ToString()) });
            }

            this.DataContext = vm;

            base.OnInitialized(e);

        }
    }
}
