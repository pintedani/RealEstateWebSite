using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models
{
    using Imobiliare.Entities;

    public class AnsambluriViewModel
    {
        public List<Ansamblu> Ansambluri { get; set; }

        public Oras Oras { get; set; }

        public int NumberOfPages { get; set; }

        public int TotalNumberOfEntries { get; set; }

        public int Page { get; set; }
    }
}