using System.Collections.Generic;

namespace Imobiliare.UI.Models
{
    using Entities;

    public class SuperAdminData : ImobilsData
    {
        public SuperAdminData(List<Imobil> imobils, ImobilFilter imobilFilter, int totalNumberOfImobils)
          : base(imobils, imobilFilter, totalNumberOfImobils)
        {
        }

        public List<EmailTemplate> EmailTemplates { get; set; }
    }
}