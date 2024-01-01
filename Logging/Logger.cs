using Imobiliare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class Logger : ILog
    {
        public void Debug(string message,
                    [CallerMemberName] string memberName = "",
                    [CallerFilePath] string filePath = "",
                    [CallerLineNumber] int lineNumber = 0)
        {
            //
        }

        //TODO Implement interface!

        public void DebugFormat(string v, string poza)
        {
            
        }

        public void DebugFormat(string v, int imobilId, string? name)
        {
            
        }

        public void DebugFormat(string v, int imobilId, string? name, string imageName)
        {
            
        }

        public void DebugFormat(string v, string poza, int anuntId, string ids)
        {
            
        }

        public void DebugFormat(string v, string userName, string? name)
        {
            
        }

        public void DebugFormat(string v, string secretNumber, int anuntId, StareAprobare stare)
        {
            
        }

        public void DebugFormat(string v)
        {
            
        }

        public void DebugFormat(string v, string title, int id)
        {
            
        }

        public void DebugFormat(string v, string emailUser, string titlu, string mesaj)
        {
            
        }

        public void ErrorFormat(string error)
        {

        }

        public void ErrorFormat(string v, string poza)
        {
            
        }

        public void ErrorFormat(string v, int anuntId, string secretNumber)
        {
            
        }

        public void ErrorFormat(string v, int anuntId, StareAprobare stareAprobare, StareAprobare stare)
        {
            
        }

        public void ErrorFormat(string v, string path, string message)
        {
            
        }

        public void ErrorFormat(string v, string nume, int judetId, Exception exception)
        {
            
        }

        public void ErrorFormat(string v, int id, string? name, string message)
        {
            
        }

        public void ErrorFormat(string v, string email, string password, object value)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(string v)
        {
            
        }

        public void InfoFormat(string v, string provider)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message)
        {
            
        }

        public void WarnFormat(string v, string userid)
        {
            
        }

        public void WarnFormat(string v, string userid, int count)
        {
            
        }

        public void WarnFormat(string v)
        {
            
        }

        public void WarnFormat(string v, int id, string titlu, string newTitle)
        {
            
        }

        public void WarnFormat(string v, string searchValue, string tipProprietate)
        {
            
        }
    }
}
