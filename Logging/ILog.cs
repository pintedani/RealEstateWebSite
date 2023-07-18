using Imobiliare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public interface ILog
    {
        void DebugFormat(string v, string poza);
        void DebugFormat(string v, int imobilId, string? name);
        void DebugFormat(string v, int imobilId, string? name, string imageName);
        void ErrorFormat(string error);
        void ErrorFormat(string v, string poza);
        void ErrorFormat(string v, int anuntId, string secretNumber);
        void ErrorFormat(string v, int anuntId, StareAprobare stareAprobare, StareAprobare stare);
        void WarnFormat(string v, string userid);
    }
}
