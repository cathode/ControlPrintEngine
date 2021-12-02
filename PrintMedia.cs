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
        public static readonly PrintMedia ThermalLabel3x1 = new PrintMedia("Label3x1", 3.0, 1.0, PrintMediaType.Thermal);
        public static readonly PrintMedia ThermalLabel4x2 = new PrintMedia("Label4x2", 4.0, 2.0, PrintMediaType.Thermal);
        public static readonly PrintMedia ThermalLabel4x6 = new PrintMedia("Label4x6", 4.0, 6.0, PrintMediaType.Thermal);

        public PrintMedia(string name, double width, double height, PrintMediaType mediaType)
        {
            this.Name = name;
            this.Width = width;
            this.Height = height;
            this.MediaType = mediaType;
        }

        public string Name { get; internal set; }

        public double Width { get; internal set; }

        public double Height { get; internal set; }

        public PrintMediaType MediaType { get; internal set; }

        static PrintMedia()
        {
            PrintMedia._registeredMedia = new List<PrintMedia>();
            PrintMedia._registeredMappings = new List<PrinterMapping>();

            // Register hardcoded medias
            PrintMedia.RegisterMedia(ThermalLabel3x1);
            PrintMedia.RegisterMedia(ThermalLabel4x2);
            PrintMedia.RegisterMedia(ThermalLabel4x6);

        }

        private static readonly List<PrintMedia> _registeredMedia;
        private static readonly List<PrinterMapping> _registeredMappings;

        public static PrintMedia RegisterMedia(string mediaName, double width, double height, PrintMediaType mediaType)
        {
            var media = new PrintMedia(mediaName, width, height, mediaType);

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
            var mapping = new PrinterMapping { PrinterPath = printerPath, MediaType = mediaType };

            PrintMedia._registeredMappings.Add(mapping);
        }

        public static PrintQueue GetPrintQueueForMediaType(PrintMediaType mediaType)
        {
            var lps = new LocalPrintServer();

            var queues = lps.GetPrintQueues();

            var mapping = _registeredMappings.FirstOrDefault(p => p.MediaType == mediaType);

            return queues.FirstOrDefault(p => p.Name == mapping.PrinterPath);

        }
    }
}
