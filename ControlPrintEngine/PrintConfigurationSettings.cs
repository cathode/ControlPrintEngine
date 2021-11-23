using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Printing;

namespace ControlPrintEngine
{
    public class PrintConfigurationSettings
    {
        public static readonly string DefaultFilePath = "ControlPrintEngine.json";

        private string filePath = PrintConfigurationSettings.DefaultFilePath;

        public string PrinterPath { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public PrintQueueWrapper ConfiguredPrinter { get; set; }

        public static PrintConfigurationSettings ReadConfigurationSettings(string configPath = null)
        {
            configPath = configPath ?? PrintConfigurationSettings.DefaultFilePath;
            PrintConfigurationSettings config;

            if (File.Exists(configPath))
            {
                var ftext = File.ReadAllText(configPath);
                config = JsonSerializer.Deserialize<PrintConfigurationSettings>(ftext);
            }
            else
            {
                config = new PrintConfigurationSettings();
            }

            config.filePath = configPath;

            // Find printer queue based on printer path
            if (!string.IsNullOrWhiteSpace(config.PrinterPath))
            {
                try
                {
                    config.ConfiguredPrinter = new PrintQueueWrapper(new LocalPrintServer().GetPrintQueue(config.PrinterPath));
                }
                catch (Exception ex)
                {
                    // error?
                }
            }

            return config;
        }

        public void WriteConfigurationSettings(string configPath = null)
        {
            // Ensure the config is syncd

            this.PrinterPath = this.ConfiguredPrinter?.PrinterName;

            var writePath = configPath ?? this.filePath;

            if (Uri.IsWellFormedUriString(writePath, UriKind.RelativeOrAbsolute))
            {
                File.WriteAllText(writePath, JsonSerializer.Serialize<PrintConfigurationSettings>(this));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
