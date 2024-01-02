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
        void Debug(string message, string memberName = "", string filePath = "", int lineNumber = 0);

        void DebugFormat(string v, string poza, int anuntId, string ids);
        void DebugFormat(string v, int imobilId, string? name);
        void DebugFormat(string v, string userName, string? name);
        void DebugFormat(string v);
        void DebugFormat(string v, string title, int id);
        void DebugFormat(string v, string emailUser, string titlu, string mesaj);
        void ErrorFormat(string error);
        void ErrorFormat(string v, string poza);
        void ErrorFormat(string v, int anuntId, string secretNumber);
        void ErrorFormat(string v, int anuntId, StareAprobare stareAprobare, StareAprobare stare);
        void ErrorFormat(string v, string path, string message);
        void ErrorFormat(string v, string nume, int judetId, Exception exception);
        void ErrorFormat(string v, int id, string? name, string message);
        void ErrorFormat(string v, string email, string password, object value);
        void InfoFormat(string v);
        void InfoFormat(string v, string provider);
        void Warn(string message);
        void WarnFormat(string v, string userid, int count);
        void WarnFormat(string v);
        void WarnFormat(string v, string userid);
        void WarnFormat(string v, int id, string titlu, string newTitle);
        void WarnFormat(string v, string searchValue, string tipProprietate);
    }
}
