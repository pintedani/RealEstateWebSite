using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imobiliare.Entities
{
    public class AnsambluriRezidentialeFilter : Filter
    {
        public AnsambluriRezidentialeFilter()
        {
            Page = 1;
            PageSize = 12;
        }

        public int OrasId { get; set; }

        public bool? Active { get; set; }
    }
}
