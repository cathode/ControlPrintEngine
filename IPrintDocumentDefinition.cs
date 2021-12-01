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
    /// <summary>
    /// Provides a contract for types implementing definitions for printable documents.
    /// </summary>
    public interface IPrintDocumentDefinition
    {
        /// <summary>
        /// Creates a new instance of the user control class representing the document's visual,
        /// and optionally populates it with the supplied data.
        /// </summary>
        /// <param name="data">Optional. An object that is used as the new control's data context.</param>
        /// <returns>A new <see cref="UserControl"/> representing the document's visual.</returns>
        UserControl CreateControl(object data = null);

        /// <summary>
        /// Gets the friendly name of the document definition.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the page size (in WPF units) of the label.
        /// </summary>
        Size PageSize { get; }

        PageOrientation Orientation { get; }
    }
}
