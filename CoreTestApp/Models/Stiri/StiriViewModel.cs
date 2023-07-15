using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imobiliare.Entities;

namespace Imobiliare.UI.Models
{
    public class StiriViewModel
    {
        public List<Stire> Stiri { get; set; }

        public int NumberOfPages { get; set; }

        public StiriFilter StiriFilter { get; set; }

        //For generic paging, which needs the property on the Model directly
        public int Page => StiriFilter.Page;
    }
}
