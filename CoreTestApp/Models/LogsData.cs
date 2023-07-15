using System;
using System.Collections.Generic;

namespace Imobiliare.UI.Models
{
    using Imobiliare.Entities;

    public class LogsData
    {
        public List<Log> Logs { get; set; }

        public DateTime SelectedDate { get; set; }

        public string SelectedSeverity { get; set; }

        public string SearchText { get; set; }

        public int NumberOfUsers { get; set; }

        public int NumberOfPages { get; set; }

        public int Page => LogsFilter.Page;

        public Filter LogsFilter { get; set; }
    }
}