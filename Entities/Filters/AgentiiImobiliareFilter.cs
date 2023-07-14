using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imobiliare.Entities
{
    public class AgentiiImobiliareFilter : Filter
    {
        public AgentiiImobiliareFilter()
        {
            this.Page = 1;
            this.PageSize = 12;
        }

        public int OrasId { get; set; }
    }
}
