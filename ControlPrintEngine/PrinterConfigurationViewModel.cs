using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ControlPrintEngine
{
    internal class PrinterConfigurationViewModel : INotifyPropertyChanged
    {
        private PrintQueueWrapper configuredPrinter;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<PrintQueueWrapper> Printers { get; set; }

        public PrintQueueWrapper ConfiguredPrinter
        {
            get
            {
                return this.configuredPrinter;
            }
            set
            {
                this.configuredPrinter = value;
                this.OnPropertyChanged();
            }
        }

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
