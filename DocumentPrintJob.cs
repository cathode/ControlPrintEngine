using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPrintEngine
{
    public class DocumentPrintJob
    {

        private readonly List<DocumentPrintJobSection> sections = new List<DocumentPrintJobSection>();

        public DocumentPrintJob()
        {

        }

        public IEnumerable<DocumentPrintJobSection> Sections { get { return this.sections.AsEnumerable(); } }

        public DocumentPrintJobSection AddSection(IPrintDocument document, IEnumerable<object> pageData = null)
        {
            var section = new DocumentPrintJobSection(this);
            this.sections.Add(section);

            section.Document = document;

            if (pageData != null)
                foreach (var p in pageData)
                    section.AddPage(p);

            return section;
        }
    }

    public class DocumentPrintJobSection
    {
        private readonly DocumentPrintJob _job;
        private readonly List<DocumentPrintJobPage> pages = new List<DocumentPrintJobPage>();

        internal DocumentPrintJobSection(DocumentPrintJob job)
        {
            this._job = job;
        }

        /// <summary>
        /// Gets or sets the <see cref="IPrintDocument"/> that will be rendered for this section
        /// </summary>
        public IPrintDocument Document { get; set; }

        public IEnumerable<DocumentPrintJobPage> Pages { get { return this.pages.AsEnumerable(); } }

        public DocumentPrintJobPage AddPage(object pageData, int count = 1, int sequence = -1)
        {
            var page = new DocumentPrintJobPage(this);

            page.Count = count;
            page.PageData = pageData;

            if (sequence < 0)
                sequence = this.pages.Count;

            page.Sequence = sequence;
            this.pages.Add(page);

            return page;
        }
    }
    public class DocumentPrintJobPage
    {
        private readonly DocumentPrintJobSection _section;
        internal DocumentPrintJobPage(DocumentPrintJobSection section)
        {
            this._section = section;
        }
        public int Count { get; set; }

        public object PageData { get; set; }

        public int Sequence { get; set; }
    }
}
