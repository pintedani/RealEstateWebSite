using System.Collections.Generic;

namespace Imobiliare.UI.Models
{
    using Imobiliare.Entities;

    public class ImobilsData
    {
        public ImobilsData(List<Imobil> imobils, ImobilFilter imobilFilter, int totalNumberOfImobils)
        {
            this.Imobils = imobils;
            this.ImobilFilter = imobilFilter;
            this.NumberOfPages = totalNumberOfImobils;
            this.ZoomLevel = 12;
        }

        public List<Imobil> Imobils { get; set; }

        //Get 2 last added imobils
        public List<Imobil> LastAddedImobils { get; set; }

        public List<Imobil> PromovatedImobils { get; set; }

        public ImobilFilter ImobilFilter { get; set; }

        public int NumberOfPages { get; set; }

        /// Total number which applies to current filter(from all pages)
        public int TotalNumberOfEntries { get; set; }

        public List<Judet> Judets { get; set; }

        public List<Oras> Orases { get; set; }

        //Based on whole Filter
        public Dictionary<Oras, int> SelectableOrasesWithNumber { get; set; }

        //Based only on judet
        public Dictionary<Oras, int> TotalSelectableOrasesWithNumber { get; set; }

        public Dictionary<Cartier, int> SelectableCartiersWithNumber { get; set; }

        public List<Cartier> Cartiers { get; set; }

        public string GpsCoordinates { get; set; }

        public int ZoomLevel { get; set; }

        public UserProfile UserProfile { get; set; }

        public int Page => ImobilFilter.Page;

        public string MapMode { get; set; }
    }
}