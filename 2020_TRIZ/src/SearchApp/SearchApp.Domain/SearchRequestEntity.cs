using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SearchApp.Domain
{
    public class SearchRequestEntity
    {
        public List<TermQueryEntity> Queries { get; set; }
    }
}