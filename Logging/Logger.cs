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
            throw new NotImplementedException();
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
    }
}
