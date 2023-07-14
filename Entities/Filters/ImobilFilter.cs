namespace Imobiliare.Entities
{
    using System;

    public class ImobilFilter : Filter
    {
        public ImobilFilter()
        {
            this.Page = 1;
            this.PageSize = 10;
        }

        public StareAprobare StareAprobare { get; set; }

        public int JudetId { get; set; }

        public int OrasId { get; set; }

        public int CartierId { get; set; }

        public TipVanzator TipVanzator { get; set; }

        //public TipOferta TipOferta { get; set; }

        public TipOfertaGen TipOfertaGen { get; set; }

        public TipProprietate TipProprietate { get; set; }

        public UserProfile UserProfile { get; set; }

        public int CamereMin { get; set; }

        public int CamereMax { get; set; }

        public int MpMin { get; set; }

        public int MpMax { get; set; }

        public int PretMin { get; set; }

        public int PretMax { get; set; }

        public DateTime PerioadaStart { get; set; }

        public DateTime PerioadaEnd { get; set; }

        public bool FiltareDupaDataAdaugareInitiala { get; set; }

        public int VechimeMaximaZile { get; set; }

        public bool OnlyWithGoogleMarker { get; set; }

        public bool OnlyUserAnunturi { get; set; }

        public bool OnlyAdminAnunturi { get; set; }

        public string GoogleMapBounds { get; set; }

        public int GetTotalNumberOfFilters
        {
            get
            {
                var filterNumber = 0;
                if (this.CamereMin > 0) filterNumber++;
                if (this.CamereMax > 0) filterNumber++;
                if (this.MpMin > 0) filterNumber++;
                if (this.MpMax > 0) filterNumber++;
                if (this.PretMin > 0) filterNumber++;
                if (this.PretMax > 0) filterNumber++;
                if (this.TipVanzator > 0) filterNumber++;
                if (this.VechimeMaximaZile > 0) filterNumber++;
                if (this.CartierId > 0) filterNumber++;

                return filterNumber;
            }
        }

        public string MapMode { get; set; }
    }
}