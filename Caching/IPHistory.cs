using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching
{
    public class IPHistory
    {
        public string Ip { get; set; }

        public int AccesTimes { get; set; }

        public DateTime LastAccess { get; set; }
    }
}
