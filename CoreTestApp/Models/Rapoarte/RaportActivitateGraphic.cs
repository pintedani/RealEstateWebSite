using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models.Rapoarte
{
    public class RaportActivitateGraphic
    {
        public DateTime DateTime { get; set; }

        public int AnunturiByUserPercentage { get; set; }

        public int AnunturiByAdminPercentage { get; set; }

        public int SesiuniDistinctePercentage { get; set; }

        public int SessionStartedDistinctCount { get; set; }

        public int AnunturiByUsersCount { get; set; }

        public int AnunturiByAdminCount { get; set; }
    }
}