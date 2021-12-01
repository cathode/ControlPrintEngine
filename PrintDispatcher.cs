using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace ControlPrintEngine
{
    public class PrintDispatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDispatcher"/> class.
        /// </summary>
        public PrintDispatcher()
        {
            // Defaults
            this.PrintDpiX = PrintDispatcher.DefaultPrintDpi;
            this.PrintDpiY = PrintDispatcher.DefaultPrintDpi;
        }

        public static readonly int DefaultPrintDpi = 300;

        public int PrintDpiX { get; set; }
        public int PrintDpiY { get; set; }
        public PrintQueue ActivePrintQueue { get; set; }

        public void Print(DocumentPrintJob job)
        {
            Contract.Requires(job != null);

        }

        public void PrintLabels(string name, object[] data, int quantity = 1)
        {
            throw new NotImplementedException();
        }

        public void PrintLabels(IPrintDocument label, IEnumerable<object> data, int quantity = 1)
        {
            Contract.Requires(label != null);
            Contract.Requires(data != null);

            this.PrintLabels(label, (IEnumerable<object>)data, quantity, Size.Empty);
        }

        public void PrintLabels(IPrintDocument def, IEnumerable<object> labelData, int quantity, Size sheetSize)
        {
            Contract.Requires(def != null);
            Contract.Requires(labelData != null);

            var fd = new FixedDocument();
            var ps = def.Stock.PageSize;
            var ss = sheetSize == Size.Empty ? ps : sheetSize;
            int cols = (int)Math.Floor(ss.Width / ps.Width);
            int rows = (int)Math.Floor(ss.Height / ps.Height);
            var elementsPerSheet = rows * cols;

            if (elementsPerSheet > 1)
            {
                var dataList = labelData.ToList();
                var sheetCount = (int)Math.Ceiling(dataList.Count / (double)elementsPerSheet);
                int j = 0;

                for (int n = 0; n < sheetCount; ++n)
                {
                    var page = new FixedPage();
                    page.Width = Math.Max(ss.Width, 0.1);
                    page.Height = Math.Max(ss.Height, 0.1);
                    page.HorizontalAlignment = HorizontalAlignment.Center;
                    page.VerticalAlignment = VerticalAlignment.Center;

                    var outer = new StackPanel();
                    Contract.Assume(page.Children != null);
                    page.Children.Add(outer);
                    outer.Orientation = Orientation.Horizontal;
                    outer.HorizontalAlignment = HorizontalAlignment.Center;
                    outer.VerticalAlignment = VerticalAlignment.Center;

                    for (int c = 0; c < cols && j < dataList.Count; c++)
                    {
                        var colPanel = new StackPanel();
                        colPanel.Orientation = Orientation.Vertical;

                        Contract.Assume(outer.Children != null);
                        outer.Children.Add(colPanel);
                        colPanel.Orientation = Orientation.Vertical;

                        for (int r = 0; r < rows && j < dataList.Count; r++)
                        {
                            var data = dataList[j++];
                            var control = def.CreateControl(data);

                            Contract.Assume(colPanel.Children != null);
                            colPanel.Children.Add(control);
                        }
                    }

                    page.Measure(ps);
                    page.Arrange(new Rect(default(Point), ss));
                    page.UpdateLayout();

                    var pc = new PageContent();
                    pc.HorizontalAlignment = HorizontalAlignment.Center;
                    pc.VerticalAlignment = VerticalAlignment.Center;
                    ((IAddChild)pc).AddChild(page);

                    Contract.Assume(fd.Pages != null);
                    fd.Pages.Add(pc);
                }
            }
            else
            {
                foreach (var data in labelData)
                {
                    var control = def.CreateControl(data);
                    var page = new FixedPage();
                    Contract.Assume(page.Children != null);
                    page.Children.Add(control);

                    page.Width = ss.Width;
                    page.Height = ss.Height;

                    page.Measure(ps);
                    page.Arrange(new Rect(default(Point), ss));
                    page.UpdateLayout();

                    var pc = new PageContent();
                    ((IAddChild)pc).AddChild(page);

                    Contract.Assume(fd.Pages != null);
                    fd.Pages.Add(pc);
                }
            }

            var dialog = new PrintDialog();

            if (this.ActivePrintQueue == null)
            {
                if (dialog.ShowDialog() == true)
                {
                    Contract.Assume(dialog.PrintTicket != null);

                    this.ActivePrintQueue = dialog.PrintQueue;
                }
                else
                {
                    return;
                }
            }
            else
            {
                dialog.PrintQueue = this.ActivePrintQueue;
            }

            // Setup and override options for label printing.
            dialog.PrintTicket.PageMediaType = PageMediaType.Label;
            dialog.PrintTicket.PageBorderless = PageBorderless.Borderless;
            dialog.PrintTicket.PageOrientation = def.Orientation;
            dialog.PrintTicket.PageResolution = new PageResolution(this.PrintDpiX, this.PrintDpiY);
            dialog.PrintTicket.PageMediaSize = new PageMediaSize(ss.Width, ss.Height);
            dialog.PrintTicket.PageScalingFactor = 100;
            dialog.PrintTicket.CopyCount = quantity;
            dialog.PrintTicket.OutputQuality = OutputQuality.High;

            // Send label to printer
            dialog.PrintDocument(fd.DocumentPaginator, def.Name);
        }

        public void SetPrinter(string printerPath)
        {
            var lps = new LocalPrintServer();

            var pq = lps.GetPrintQueue(printerPath);

            this.ActivePrintQueue = pq;
        }
    }
}
