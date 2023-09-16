using Imobiliare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class Logger : ILog
    {
        public void Debug(string v)
        {
            //throw new NotImplementedException();
        }

        //TODO Implement inteface!

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
            throw new NotImplementedException();
        }

        public void DebugFormat(string v, string userName, string? name)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(string v, string secretNumber, int anuntId, StareAprobare stare)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(string v)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(string v, string title, int id)
        {
            throw new NotImplementedException();
        }

        public void DebugFormat(string v, string emailUser, string titlu, string mesaj)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void ErrorFormat(string v, string nume, int judetId, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(string v, int id, string? name, string message)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(string v)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string v, string userid)
        {
            
        }

        public void WarnFormat(string v, string userid, int count)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string v)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string v, int id, string titlu, string newTitle)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string v, string searchValue, string tipProprietate)
        {
            throw new NotImplementedException();
        }
    }
}
