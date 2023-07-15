using Imobiliare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models.DTOs
{
    public class LinkExternChromeExtDto
    {
        public string Link { get; set; }

        public string Oras { get; set; }

        public string Telefon { get; set; }

        public string EmailAdmin { get; set; }

        public int Valabilitate { get; set; }

        public TipProprietate TipProprietate { get; set; }

        public TipOfertaGen TipOfertaGen { get; set; }

        public bool Negociabil { get; set; }

        public bool CT { get; set; }

        public bool Finisat { get; set; }
    }
}