using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imobiliare.Entities;
using Imobiliare.UI.Models.Rapoarte;

namespace Imobiliare.UI.Models
{
    public class RaportActivitateData
    {
        public List<RaportActivitateGraphic> RaportActivitateGraphics { get; set; }

        public List<RaportActivitate> RaportActivitates { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}