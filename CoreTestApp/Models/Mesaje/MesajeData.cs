using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imobiliare.Entities;

namespace Imobiliare.UI.Models
{
    public class MesajeData
    {
        public List<Mesaj> Mesaje { get; set; }

        public int NumberOfPages { get; set; }

        public int Page { get; set; }
    }
}