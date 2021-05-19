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
using System.Windows.Controls.Primitives;
using System.Printing;

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

        private PrinterConfigurationViewModel vm;


        protected override void OnInitialized(EventArgs e)
        {
            //var pq = new PrintQueue()
            this.vm = new PrinterConfigurationViewModel();

            vm.Printers = new List<PrintQueueWrapper>();

            foreach (var pq in new LocalPrintServer().GetPrintQueues())
            {
                this.vm.Printers.Add(new PrintQueueWrapper(pq));
            }

            this.DataContext = this.vm;

            this.selectPrinterButton.IsEnabled = false;

            base.OnInitialized(e);

        }

        private void selectPrinterButton_Click(object sender, RoutedEventArgs e)
        {
            var pqw = this.availablePrinters.SelectedItem as PrintQueueWrapper;

            if (pqw != null && pqw != this.vm.ConfiguredPrinter)
            {
                this.vm.ConfiguredPrinter = pqw;

                this.selectPrinterButton.IsEnabled = false;
            }
        }

        private void printTestPageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void availablePrinters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pqw = this.availablePrinters.SelectedItem as PrintQueueWrapper;

            if (pqw != null)
            {
                if (pqw != this.vm.ConfiguredPrinter)
                {
                    this.selectPrinterButton.IsEnabled = true;
                }
            }
            else
            {
                this.selectPrinterButton.IsEnabled = false;
            }
        }
    }
}
