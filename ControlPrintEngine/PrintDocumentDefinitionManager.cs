using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ControlPrintEngine
{
    public class PrintDocumentDefinitionManager
    {
        private static readonly Lazy<PrintDocumentDefinitionManager> instance = new Lazy<PrintDocumentDefinitionManager>(() => new PrintDocumentDefinitionManager());

        private readonly Dictionary<string, IPrintDocumentDefinition> definitions;

        private PrintDocumentDefinitionManager()
        {
            this.definitions = new Dictionary<string, IPrintDocumentDefinition>();
        }

        public static PrintDocumentDefinitionManager Instance
        {
            get
            {
                return instance.Value;
            }
        }

        private void ScanAssemblyForDefinitions(Assembly asm = null)
        {
            if (asm == null)
            {
                asm = Assembly.GetExecutingAssembly();
            }

            var types = asm.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Contains(typeof(IPrintDocumentDefinition)));

            foreach (var def in types)
            {
                var inst = Activator.CreateInstance(def);
            }
        }
    }
}
