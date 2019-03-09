using System;
using System.Collections.Generic;
using System.Text;

namespace MutantsCatalogue.Domain
{
    public class DomainSettings
    {
        public DomainSettings(string copyright, string copyrightYear)
        {
            Copyright = copyright;
            CopyrightYear = copyrightYear;
        }
        public string Copyright { get; }
        public string CopyrightYear { get; }
    }
}
