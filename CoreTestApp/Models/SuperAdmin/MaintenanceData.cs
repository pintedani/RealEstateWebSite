using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models.SuperAdmin
{
    public class MaintenanceData
    {
        public int UsedDbInMb { get; set; }

        public int MaxDbSizeInMb { get; set; }

        public int UsedPercentage => (int) ((double) UsedDbInMb / MaxDbSizeInMb * 100);
    }
}