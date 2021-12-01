using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ControlPrintEngine
{
    /// <summary>
    /// Provides a system for discovery of print documents in an assembly.
    /// </summary>
    public class PrintDocumentDefinitionManager
    {
        private static readonly Lazy<PrintDocumentDefinitionManager> instance = new Lazy<PrintDocumentDefinitionManager>(() => new PrintDocumentDefinitionManager());

        private readonly Dictionary<string, IPrintDocument> definitions;

        private PrintDocumentDefinitionManager()
        {
            this.definitions = new Dictionary<string, IPrintDocument>();
        }

        public static PrintDocumentDefinitionManager Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public void ScanAssemblyForDefinitions(Assembly asm = null)
        {
            if (asm == null)
                asm = Assembly.GetExecutingAssembly();

            var types = asm.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Contains(typeof(IPrintDocument)));

            foreach (var def in types)
            {
                var inst = Activator.CreateInstance(def);
            }
        }
    }
}
