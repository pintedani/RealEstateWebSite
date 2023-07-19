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
        void Debug(string v);
        void DebugFormat(string v, string poza, int anuntId, string ids);
        void DebugFormat(string v, int imobilId, string? name);
        void DebugFormat(string v, int imobilId, string? name, string imageName);
        void DebugFormat(string v, string userName, string? name);
        void DebugFormat(string v, string? name);
        void DebugFormat(string v, string secretNumber, int anuntId, StareAprobare stare);
        void DebugFormat(string v);
        void ErrorFormat(string error);
        void ErrorFormat(string v, string poza);
        void ErrorFormat(string v, int anuntId, string secretNumber);
        void ErrorFormat(string v, int anuntId, StareAprobare stareAprobare, StareAprobare stare);
        void InfoFormat(string v);
        void WarnFormat(string v, string userid, int count);
        void WarnFormat(string v);
        void WarnFormat(string v, string userid);
        void WarnFormat(string v, int id, string titlu, string newTitle);
        void WarnFormat(string v, string searchValue, string tipProprietate);
    }
}
