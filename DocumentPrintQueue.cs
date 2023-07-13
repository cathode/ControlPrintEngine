using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    /// <summary>
    /// Provides a queue for processing <see cref="DocumentPrintJob"/> items.
    /// </summary>
    public class DocumentPrintQueue
    {
        private readonly ConcurrentQueue<DocumentPrintJob> _jobQueue = new ConcurrentQueue<DocumentPrintJob>();

        private Thread _processingThread;
        private bool _isProcessing;
        private AutoResetEvent _interlock = new AutoResetEvent(true);
        //private string _printerName;

        public PrintQueue UnderlyingQueue { get; set; }

        public string PrinterPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentPrintQueue"/> class.
        /// </summary>
        public DocumentPrintQueue()
        {
        }

        public DocumentPrintQueue(string printerPath)
        {
            this.PrinterPath = printerPath;
        }

        public static readonly int DefaultPrintDpi = 300;

        public int PrintDpiX => 300;
        public int PrintDpiY => 300;

        private void ProcessQueue()
        {
            if (this._interlock.WaitOne(1))
            {
                if (this._processingThread == null || !this._processingThread.IsAlive)
                {
                    this._processingThread = new Thread(new ThreadStart(this.ProcessQueue_ThreadStart));
                    this._processingThread.SetApartmentState(ApartmentState.STA);
                    this._isProcessing = true;
                    this._processingThread.Start();
                }
            }
        }

        private void ProcessQueue_ThreadStart()
        {
            try
            {
                DocumentPrintJob job;

                while (this._jobQueue.TryDequeue(out job))
                {
                    this.DoPrintJob(job);
                }
            }
            finally
            {
                this._isProcessing = false;
                this._interlock.Set();
            }
        }

        private void DoPrintJob(DocumentPrintJob job)
        {
            if (this.UnderlyingQueue == null)
            {
                if (this.PrinterPath == null)
                {
                    this.UnderlyingQueue = PrintMedia.GetPrintQueueForMediaType(PrintMediaType.Thermal);
                }
                else
                {
                using (var lps = new LocalPrintServer())
                    {
                        var queues = lps.GetPrintQueues();

                        this.UnderlyingQueue = queues.FirstOrDefault(p => p.Name == this.PrinterPath);
                    }
                }
                this.UnderlyingQueue.Refresh();
            }

            foreach (var section in job.Sections)
                this.DoPrintSection(section);
        }

        private void DoPrintSection(DocumentPrintJobSection section)
        {
            var fd = new FixedDocument();
            var ps = new Size(section.Document.Stock.Width * this.PrintDpiX, section.Document.Stock.Height * this.PrintDpiY);

            foreach (var pg in section.Pages)
            {
                var control = section.Document.CreateControl(pg.PageData);
                var pageContainer = new FixedPage();

                pageContainer.Children.Add(control);

                pageContainer.Width = ps.Width;
                pageContainer.Height = ps.Height;

                pageContainer.Measure(ps);
                pageContainer.Arrange(new Rect(default(Point), ps));
                pageContainer.UpdateLayout();

                var pc = new PageContent();
                ((IAddChild)pc).AddChild(pageContainer);

                Contract.Assume(fd.Pages != null);
                for (int i = 0; i < pg.Count; ++i)
                    fd.Pages.Add(pc);
            }


            //pq.UserPrintTicket.

            var dialog = new PrintDialog() { PrintQueue = this.UnderlyingQueue };

            // Setup and override options for label printing.
            dialog.PrintTicket.PageMediaType = PageMediaType.Label;
            dialog.PrintTicket.PageBorderless = PageBorderless.Borderless;
            dialog.PrintTicket.PageOrientation = section.Document.Orientation;
            dialog.PrintTicket.PageResolution = new PageResolution(this.PrintDpiX, this.PrintDpiY);
            dialog.PrintTicket.PageMediaSize = new PageMediaSize(ps.Width, ps.Height);
            dialog.PrintTicket.PageScalingFactor = 100;
            dialog.PrintTicket.CopyCount = 1;
            dialog.PrintTicket.OutputQuality = OutputQuality.High;

            var dc = fd.DataContext;
            fd.DataContext = null;
            fd.DataContext = dc;

            // Send label to printer
            dialog.PrintDocument(fd.DocumentPaginator, section.Document.Name);
        }

        /// <summary>
        /// Processes and sends the print job to the printer device and waits for the job to complete.
        /// </summary>
        /// <param name="job"></param>
        public void Print([NotNull] DocumentPrintJob job)
        {
            this._jobQueue.Enqueue(job);

            this.ProcessQueue();
        }

        public async Task PrintAsync(DocumentPrintJob job)
        {
            throw new NotImplementedException();
        }
    }
}
