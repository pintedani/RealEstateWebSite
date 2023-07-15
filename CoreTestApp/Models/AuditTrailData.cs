using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imobiliare.Entities;

namespace Imobiliare.UI.Models
{
    public class AuditTrailData
    {
        public List<AuditTrail> AuditTrails { get; set; }

        public DateTime SelectedDate { get; set; }

        public string SearchText { get; set; }

        public int NumberOfUsers { get; set; }

        public int NumberOfPages { get; set; }

        public int Page => LogsFilter.Page;

        public Filter LogsFilter { get; set; }
    }
}