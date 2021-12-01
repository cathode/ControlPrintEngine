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
    public class PrintMedia
    {
        public static readonly PrintMedia ThermalLabel3x1 = new PrintMedia("TL3x1", 3.0 * 96, 1.0 * 96);
        public static readonly PrintMedia ThermalLabel4x2 = new PrintMedia("TL4x2", 4.0 * 96, 2.0 * 96);
        public static readonly PrintMedia ThermalLabel4x6 = new PrintMedia("TL4x6", 4.0 * 96, 6.0 * 96);

        public PrintMedia(string name, double width, double height)
        {
            this.Name = name;
            this.Width = width;
            this.Height = height;
            //this.Orientation = PageOrientation.Portrait;
        }

        public string Name { get; internal set; }

        public double Width { get; internal set; }

        public double Height { get; internal set; }

        /// <summary>
        /// Gets the page size (in WPF units) of the label.
        /// </summary>
        //[System.Text.Json.Serialization.JsonIgnore]
        //public Size PageSize { get; internal set; }

        static PrintMedia()
        {
            PrintMedia._registeredMedia = new List<PrintMedia>();

            // Register hardcoded medias
            PrintMedia.RegisterMedia(ThermalLabel3x1);
            PrintMedia.RegisterMedia(ThermalLabel4x2);
            PrintMedia.RegisterMedia(ThermalLabel4x6);

        }

        private static readonly List<PrintMedia> _registeredMedia;
        private static readonly List<PrinterMapping> _registeredMappings;

        public static PrintMedia RegisterMedia(string mediaName, float width, float height, PageOrientation orientation)
        {
            var media = new PrintMedia(mediaName, width, height);

            return PrintMedia.RegisterMedia(media);
        }

        public static PrintMedia RegisterMedia(PrintMedia media)
        {
            if (!_registeredMedia.Contains(media) && !_registeredMedia.Any(p => p.Name == media.Name))
            {
                PrintMedia._registeredMedia.Add(media);

                return media;
            }

            return null;
        }
        public static PrintMedia GetMedia(string mediaName)
        {
            return _registeredMedia.Find(p => p.Name == mediaName);
        }

        public static PrintQueue GetPrintQueueForMedia(string mediaName)
        {
            return null;
        }

        public static PrintQueue GetPrintQueueForMedia(PrintMedia media)
        {
            return null;
        }

        public static void RegisterPrinter(string printerPath, PrintMediaType mediaType)
        {
            var mapping = new PrinterMapping { PrinterPath = printerPath };
        }

        public static PrintQueue GetPrintQueueForMediaType(PrintMediaType thermal)
        {
            var lps = new LocalPrintServer();

            var queues = lps.GetPrintQueues();

            return queues.FirstOrDefault(p => p.Name == "ZDesigner ZM600 300 dpi (ZPL)");

        }
    }
}
